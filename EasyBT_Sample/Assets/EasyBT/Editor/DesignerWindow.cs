using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEditor.ShortcutManagement;
using EasyBT.Runtime;


namespace EasyBT.Editor
{
    public enum WindowState
    {
        None = 0,
        Connecting = 1,
        NotSelectGameObject = 2,
        Create = 3,
        SelectedGameObejctIsPrefabAsset = 4,
    }

    public enum ContextMenuType
    {
        None = 0,
        Create = 1,
        Connect = 2,
    }

    public class DesignerWindow : EditorWindow
    {
        private const float kZoomMin = 0.2f;
        private const float kZoomMax = 2f;

        private int dragViewGroupIndex;
        [SerializeField]
        private GameObject target;
        public static DesignerWindow window;
        private KeyCode currentHotKey = KeyCode.None;
        private WindowState windowState = WindowState.None;
        private ContextMenuType contextMenuType = ContextMenuType.None;
        private GenericMenu createMenu;
        private GenericMenu connectMenu;
        private Rect selectFrameRect;
        private Rect zoomArea;
        private float zoom = 1.0f;
        public Vector2 zoomCoordsOrigin = Vector2.zero;

        [MenuItem("EasyBT/Behavior Designer")]
        public static void Init()
        {
            window = GetWindow<DesignerWindow>("EasyBT");
            window.minSize = new Vector2(500, 500);
            window.wantsMouseEnterLeaveWindow = true;
        }

        private void OnEnable()
        {
            window = this;
            RegisterEvent();


            if (target != null)
            {
                var tree = target.GetComponent<BehaviorTree>();
                NodeManager.instance.CreateNodeManager(tree);
                return;
            }

            SetWindowStateAndTarget();
            GetData();
        }

        private void OnSelectionChange()
        {
            //防止編輯節點的時候改動Inspector被當作選擇更改而錯誤儲存
            if (Selection.activeObject != null && Selection.activeObject.GetType().Name == "NodeInspector")
                return;

            if (target != null && target.gameObject != Selection.activeGameObject)
            {
                NodeManager.instance.Save();
            }

            SetWindowStateAndTarget();
            GetData();
        }

        private void OnDisable()
        {
            Selection.activeObject = null;
            NodeManager.instance.Save();
            ClearEvent();
        }


        private void OnDestroy()
        {
            window = null;
        }


        private void GetNewDataOnChangedPlayMode(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.ExitingEditMode:
                    NodeManager.instance.Save();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    if (target != null)
                    {
                        //進入播放模式取得的data是副本,這副本將在退出時被清除,所以再撥放模式更動行為樹將不被視為更改樹
                        var tree = target.GetComponent<BehaviorTree>();
                        Selection.activeGameObject = tree.gameObject;
                        NodeManager.instance.CreateNodeManager(tree);
                        break;
                    }
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    if (target != null)
                    {
                        var tree = target.GetComponent<BehaviorTree>();
                        if (tree == null)
                            break;
                        Selection.activeGameObject = tree.gameObject;
                        NodeManager.instance.CreateNodeManager(tree);
                        break;
                    }
                    break;
            }

            Repaint();
        }

        private void SetWindowStateAndTarget()
        {
            if (Selection.activeGameObject == null)
            {
                target = null;
                windowState = WindowState.NotSelectGameObject;
            }
            else if (PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject))
            {
                target = null;
                windowState = WindowState.SelectedGameObejctIsPrefabAsset;
            }
            else
            {
                target = Selection.activeGameObject;
            }
        }

        private void GetData()
        {

            NodeManager.instance.onAddNewScript -= Repaint;

            if (Selection.activeGameObject != null && !PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject))
            {
                BehaviorTree behaviorTree = Selection.activeGameObject.GetComponent<BehaviorTree>();

                if (behaviorTree == null || behaviorTree.behaviorData == null)
                {
                    windowState = WindowState.Create;
                }
                else
                {

                    NodeManager.instance.CreateNodeManager(behaviorTree);
                    windowState = WindowState.None;
                    currentHotKey = KeyCode.None;
                    NodeManager.instance.onAddNewScript += Repaint;
                }
            }

            Repaint();
        }

        private void OnGUI()
        {

            if (windowState == WindowState.NotSelectGameObject)
            {
                Drawer.DrawUnselectedGameObjectView(position.width, position.height);

                return;
            }
            else if (windowState == WindowState.SelectedGameObejctIsPrefabAsset)
            {
                Drawer.DrawSelectedIsPrefabAssetView(position.width, position.height);
                return;
            }
            else if (windowState == WindowState.Create)
            {
                if (Drawer.DrawCreateBehaviorTreeView(position.width, position.height))
                {
                    CreateBehaviorTreeAndData();
                }

                return;
            }

            HandleEvents();

            if (Event.current.type == EventType.Repaint)
            {
                Drawer.DrawBackGround(position.width, position.height, NodeStyle.blackBackground);
                zoomArea = new Rect(0, 21, Screen.width, Screen.height);
                EditorZoomArea.Begin(zoom, zoomArea);
                Drawer.DrawGrids(100, new Color(0, 0, 0, 0.2f), zoomCoordsOrigin, zoom);// Small grid
                Drawer.DrawGrids(200, new Color(0, 0, 0, 0.4f), zoomCoordsOrigin, zoom); // Big grid
                if (target != null)
                {
                    var data = NodeManager.instance.GetData();

                    if (data != null)
                    {
                        Drawer.DrawNodes(NodeManager.instance.GetData().nodes, zoomCoordsOrigin);
                        Drawer.DrawSelectedNodeFrame(NodeManager.instance.GetSelectedNodes(), zoomCoordsOrigin);
                        Drawer.DrawConnectLines(NodeManager.instance.GetData().connectLines, zoomCoordsOrigin);
                    }
                }
            }

            if (windowState == WindowState.Connecting)
            {
                if (NodeManager.instance.GetSelectedNodes()[0] != null)
                {
                    Vector2 startPos = NodeManager.instance.GetSelectedNodes()[0].rect.center - zoomCoordsOrigin;
                    Drawer.DrawConnectLine(startPos, Event.current.mousePosition, Color.white, 3.5f);
                    Repaint();
                }

            }

            EditorZoomArea.End();
            Drawer.DrawSelectFrame(selectFrameRect);
            Repaint();
        }

        private void InitConnectMenu()
        {
            connectMenu = new GenericMenu();
            contextMenuType = ContextMenuType.Connect;
            connectMenu.AddItem(new GUIContent($"Connecting"), false,
                () =>
                {
                    windowState = WindowState.Connecting;
                }
                );
        }

        private void HandleEvents()
        {
            switch (Event.current.type)
            {
                case EventType.ContextClick:
                    if (!EditorApplication.isPlaying)
                        ShowContextMenu();
                    break;
                case EventType.KeyDown:

                    if (Event.current.keyCode != KeyCode.None)
                    {
                        if (Event.current.keyCode == KeyCode.Delete)
                        {
                            if (!EditorApplication.isPlaying)
                                NodeManager.instance.DelectSelected();
                        }
                        else if (Event.current.keyCode == KeyCode.C && Event.current.control)
                        {
                            if (!EditorApplication.isPlaying)
                                NodeManager.instance.Copy();
                        }
                        else if (Event.current.keyCode == KeyCode.V && Event.current.control)
                        {
                            if (!EditorApplication.isPlaying)
                                NodeManager.instance.Paste();
                        }

                        currentHotKey = Event.current.keyCode;
                        Repaint();
                    }

                    break;
                case EventType.KeyUp:
                    currentHotKey = KeyCode.None;
                    break;
                case EventType.MouseDown:
                    if (Event.current.button == 0)
                    {
                        //禁用undo操作
                        /*
                        if (ShortcutManager.instance.activeProfileId == "Disable")
                            ShortcutManager.instance.DeleteProfile("Disable");
                        ShortcutManager.instance.CreateProfile("Disable");
                        ShortcutManager.instance.activeProfileId = "Disable";
                        ShortcutManager.instance.RebindShortcut("Main Menu/Edit/Undo", new ShortcutBinding());  */

                        //判斷是否點到節點或連接線
                        var node = NodeManager.instance.GetNodeOfMousePos(ConvertViewToWorld(Event.current.mousePosition));
                        var connectLine = NodeManager.instance.GetConnectLineOfMousePos
                            (ConvertViewToWorld(Event.current.mousePosition));

                        if (node != null)
                        {
                            if (windowState == WindowState.Connecting)
                            {
                                NodeManager.instance.ConnectNode(node);
                                windowState = WindowState.None;
                            }
                            else if (windowState == WindowState.None)
                            {
                                if (NodeManager.instance.GetSelectedNodes().Count > 1
                                    && NodeManager.instance.GetSelectedNodes().Contains(node))
                                {
                                    if (Event.current.clickCount >= 2)
                                        NodeManager.instance.CancelSelected();
                                }
                                else
                                {
                                    NodeManager.instance.SelectNode(node, !Event.current.control);
                                }
                            }

                        }
                        else if (connectLine != null)
                        {
                            NodeManager.instance.SelectConnectLine(connectLine, true);
                        }
                        else
                        {
                            NodeManager.instance.CancelSelected();
                            windowState = WindowState.None;
                            selectFrameRect.position = Event.current.mousePosition;
                        }

                    }
                    else if (Event.current.button == 1)
                    {
                        //判斷是否點到節點
                        var node = NodeManager.instance.GetNodeOfMousePos(ConvertViewToWorld(Event.current.mousePosition));

                        if (node != null)
                        {

                            NodeManager.instance.SelectNode(node, true);
                            InitConnectMenu();

                        }
                        else
                        {
                            NodeManager.instance.CancelSelected();
                            InitCreateMenu(ConvertViewToWorld(Event.current.mousePosition));
                        }
                    }

                    Event.current.Use();
                    break;
                case EventType.MouseDrag:
                    if (currentHotKey == KeyCode.W)
                    {
                        Undo.RecordObject(this, "DragView");
                        if (dragViewGroupIndex == 0)
                            dragViewGroupIndex = Undo.GetCurrentGroup();
                        zoomCoordsOrigin -= Event.current.delta / zoom;
                    }
                    else if (currentHotKey != KeyCode.W)
                    {
                        //Drag currently node
                        if (NodeManager.instance.GetSelectedNodes().Count > 0 && selectFrameRect == Rect.zero)
                            NodeManager.instance.MoveSelectedNodePos(Event.current.delta / zoom);

                        else if (selectFrameRect.position != Vector2.zero)
                        {
                            selectFrameRect.size += Event.current.delta;
                            Vector2 rectWorldCoords = ConvertViewToWorld(selectFrameRect.position);
                            var correctionRect = new Rect(rectWorldCoords, selectFrameRect.size / zoom);
                            NodeManager.instance.SelectNodes(correctionRect);
                        }
                    }
                    Repaint();
                    Event.current.Use();
                    break;
                case EventType.MouseUp:
                    selectFrameRect = Rect.zero;
                    if (dragViewGroupIndex != 0)
                    {
                        Undo.CollapseUndoOperations(dragViewGroupIndex);
                        dragViewGroupIndex = 0;
                    }
                    /*
                    if (ShortcutManager.instance.activeProfileId == "Disable")
                    {
                        ShortcutManager.instance.DeleteProfile("Disable");
                    } */

                    Repaint();
                    break;
                case EventType.ScrollWheel:

                    Vector2 viewCoordsMousePos = Event.current.mousePosition;
                    Vector2 delta = Event.current.delta;
                    Vector2 zoomCoordsMousePos = ConvertViewToWorld(viewCoordsMousePos);
                    float zoomDelta = -delta.y * 0.015f;
                    float oldZoom = zoom;
                    zoom += zoomDelta;
                    zoom = Mathf.Clamp(zoom, kZoomMin, kZoomMax);
                    Vector2 originOffset = (zoomCoordsMousePos - zoomCoordsOrigin)
                       - (oldZoom / zoom) * (zoomCoordsMousePos - zoomCoordsOrigin);
                    zoomCoordsOrigin += originOffset;
                    Event.current.Use();
                    break;

                case EventType.MouseLeaveWindow:
                    if (selectFrameRect.position != Vector2.zero)
                        selectFrameRect = Rect.zero;
                    /*
                    if (ShortcutManager.instance.activeProfileId == "Disable")
                    {
                        ShortcutManager.instance.DeleteProfile("Disable");
                    }*/
                    break;


            }
        }

        private void InitCreateMenu(Vector2 mousePos)
        {
            createMenu = new GenericMenu();
            contextMenuType = ContextMenuType.Create;
            var assembly = Assembly.GetAssembly(typeof(Node));

            foreach (var item in assembly.GetTypes())
            {

                var attribute = (CustomNodeAttibute)item.GetCustomAttribute(typeof(CustomNodeAttibute));

                if (attribute != null)
                {
                    createMenu.AddItem(new GUIContent($"CreateNode/{attribute.type.Name}/{item.Name}"), false,
                        () =>
                        {
                            var node = NodeManager.CreateNodeFromString
                            (new Rect(mousePos, new Vector2(100, 100)), item.Name);
                            NodeManager.instance.JoinNode(node);
                        });
                }
            }
        }

        private void CreateBehaviorTreeAndData()
        {
            BehaviorTree behaviorTree = Selection.activeGameObject.GetComponent<BehaviorTree>();

            //The target has behaviorTree but has not behaviorData
            if (behaviorTree != null)
            {
                behaviorTree.behaviorData = new BehaviorData();
                var entryNode = NodeManager.CreateNodeFromString
                    (new Rect(position.width / 2, position.height / 5, 100, 100), "EntryNode");
                behaviorTree.behaviorData.nodes.Add(entryNode);

            }
            else
            {
                behaviorTree = Selection.activeGameObject.AddComponent<BehaviorTree>();
                behaviorTree.behaviorData = new BehaviorData();
                var entryNode = NodeManager.CreateNodeFromString
                    (new Rect(position.width / 2, position.height / 5, 100, 100), "EntryNode");
                behaviorTree.behaviorData.nodes.Add(entryNode);
            }

            EditorUtility.SetDirty(behaviorTree);//如果對象是一個prefab instance 必須將修改資料保存回
            GetData();
        }

        private void ShowContextMenu()
        {
            switch (contextMenuType)
            {
                case ContextMenuType.Create:
                    createMenu.ShowAsContext();
                    break;
                case ContextMenuType.Connect:
                    connectMenu.ShowAsContext();
                    break;
            }

            contextMenuType = ContextMenuType.None;
        }

        public Vector2 ConvertViewToWorld(Vector2 viewCoords)
        {
            return viewCoords / zoom + zoomCoordsOrigin;
        }

        public Vector2 ConvertWorldToView(Vector2 worldCoords)
        {
            return (worldCoords - zoomCoordsOrigin) * zoom;
        }

        private void RegisterEvent()
        {
            Node.nodeStateChanged += Repaint;
            Undo.undoRedoPerformed += Repaint;
            EditorApplication.playModeStateChanged += GetNewDataOnChangedPlayMode;
        }

        private void ClearEvent()
        {
            Node.nodeStateChanged -= Repaint;
            Undo.undoRedoPerformed -= Repaint;
            EditorApplication.playModeStateChanged -= GetNewDataOnChangedPlayMode;
        }

    }

}


