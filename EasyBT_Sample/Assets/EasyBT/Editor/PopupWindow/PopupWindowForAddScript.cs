using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System;
using System.Reflection;
using EasyBT.Runtime;

namespace EasyBT.Editor
{
    public class PopupWindowForAddScript : PopupWindowContent
    {

        public class Item
        {

            public Rect rect;
            public string scriptName;

            public Item(Rect rect, string scriptName)
            {
                this.rect = rect;
                this.scriptName = scriptName;
            }
        }

        public enum FindType
        {
            Act = 0,
            Condition = 1,
        }

        private Assembly assembly;
        private Type[] types;
        private FindType findType;
        private Vector2 windowSize;
        private SearchField searchField;
        private string keyWord = "";
        private Vector2 scrollViewPos;
        private List<Item> items = new List<Item>();
        private Dictionary<string, MonoScript> monoScripts = new Dictionary<string, MonoScript>();

        public PopupWindowForAddScript(Vector2 windowSize, FindType findType)
        {
            this.windowSize = windowSize;
            searchField = new SearchField();
            this.findType = findType;
        }

        public override Vector2 GetWindowSize()
        {
            return windowSize;
        }

        public override void OnOpen()
        {
            string[] assetGuids = AssetDatabase.FindAssets($"t:monoScript"
                , new string[] { $"Assets/EasyBT/EasyBTScripts/{this.findType.ToString()}" });

            if (assetGuids.Length == 0)
                return;
            else
            {
                foreach (var guid in assetGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                    if (monoScript.name == monoScript.GetClass()?.Name)
                    {
                        monoScripts.Add(monoScript.name, monoScript);
                    }
                }
            }     
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Space(5);
            keyWord = searchField.OnGUI(keyWord);

            scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);
            HandleEvents();
            if (keyWord == "")
            {
                foreach (var script in monoScripts)
                {
                    GUILayout.BeginHorizontal(NodeStyle.itemBtn);
                    GUILayout.Box("", NodeStyle.itemIcon, GUILayout.Width(15), GUILayout.Height(15));
                    GUILayout.Label(script.Key);
                    GUILayout.EndHorizontal();
                    if (Event.current.type == EventType.Repaint)
                        items.Add(new Item(GUILayoutUtility.GetLastRect(), script.Key));
                }
            }
            else
            {
                List<string> containsKeyWordList = new List<string>();

                foreach (var script in monoScripts)
                {
                    if (script.Key.ToUpper().Contains(keyWord.ToUpper()))
                    {
                        containsKeyWordList.Add(script.Key);
                    }
                }

                foreach (var scriptName in containsKeyWordList)
                {
                    GUILayout.BeginHorizontal(NodeStyle.itemBtn);
                    GUILayout.Box("", NodeStyle.itemIcon, GUILayout.Width(15), GUILayout.Height(15));
                    GUILayout.Label(scriptName);
                    GUILayout.EndHorizontal();
                    if (Event.current.type == EventType.Repaint)
                        items.Add(new Item(GUILayoutUtility.GetLastRect(), scriptName));
                }
            }
            GUILayout.EndScrollView();
        }

        private void HandleEvents()
        {
            if (Event.current.type == EventType.MouseMove)
                this.editorWindow.Repaint();
            else if (Event.current.type == EventType.MouseDown)
            {
                foreach (var item in items)
                {

                    if (item.rect.Contains(Event.current.mousePosition))
                    {
                        NodeManager.instance.AddScriptToCurrentNode(monoScripts[item.scriptName]);
                        this.editorWindow.Close();
                    }
                }
            }
            else if (Event.current.type == EventType.Repaint)
                items.Clear();
        }

    }
}

