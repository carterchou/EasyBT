using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    public class NodeStyle
    {
        private static Texture _scriptTexture;
        public static Texture scriptTexture
        {
            get
            {
                if (_scriptTexture == null)
                {
                    _scriptTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/EasyBT/Editor/Texture/Gizmos/ScriptIcon.png");
                }

                return _scriptTexture;
            }
        }

        private static GUIStyle _warningSign;
        public static GUIStyle warningSign
        {
            get
            {
                if (_warningSign == null)
                {
                    _warningSign = new GUIStyle();
                    _warningSign.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>
                        ("Assets/EasyBT/Editor/Texture/Gizmos/WarningSign.png");
                }

                return _warningSign;
            }
        }

        private static GUIStyle _testStyle;
        public static GUIStyle testStyle
        {
            get
            {
                if (_testStyle == null)
                {
                    _testStyle = new GUIStyle();
                    _testStyle.onNormal.background = GetTexture(Color.white);
                    _testStyle.normal.background = GetTexture(Color.black);
                }

                return _testStyle;
            }
        }

        private static GUIStyle _itemIcon;
        public static GUIStyle itemIcon
        {
            get
            {
                if (_itemIcon == null)
                {
                    _itemIcon = new GUIStyle();
                    _itemIcon.padding = new RectOffset(2, 0, 0, 0);
                    _itemIcon.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>
                        ("Assets/EasyBT/Editor/Texture/Gizmos/ScriptIcon.png");
                }

                return _itemIcon;
            }
        }

        private static GUIStyle _ItemBtn;
        public static GUIStyle itemBtn
        {
            get
            {
                if (_ItemBtn == null)
                {
                    _ItemBtn = new GUIStyle();
                    _ItemBtn.normal.textColor = Color.black;
                    _ItemBtn.alignment = TextAnchor.MiddleCenter;
                    _ItemBtn.normal.background = GetTexture(Color.clear);
                    _ItemBtn.hover.textColor = Color.white;
                    _ItemBtn.hover.background = GetTexture(new Color(0.45f, 0.7f, 1, 1f));
                    _ItemBtn.fontSize = 15;
                    _ItemBtn.padding = new RectOffset(2, 2, 2, 2);
                }

                return _ItemBtn;
            }
        }
        private static GUIStyle _selectFrame;
        public static GUIStyle selectFrame
        {
            get
            {
                //在退出播放模式後Texture會被釋放掉,必須重新創建
                if (_selectFrame == null || _selectFrame.normal.background == null)
                {
                    _selectFrame = new GUIStyle();
                    _selectFrame.normal.background = GetTexture(new Color(0.6f, 0.8f, 1f, 0.4f));
                }

                return _selectFrame;
            }
        }


        private static Texture2D _blackBackground;
        public static Texture2D blackBackground
        {
            get
            {
                if (_blackBackground == null)
                {
                    _blackBackground = GetTexture(new Color(0.35f, 0.35f, 0.35f));
                }

                return _blackBackground;
            }
        }

        private static Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private static GUIStyle _noneNodeStyle;

        public static GUIStyle noneNodeStyle
        {
            get
            {
                if (_noneNodeStyle == null)
                {
                    _noneNodeStyle = new GUIStyle();
                    _noneNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath
                        ("Assets/EasyBT/Editor/Texture/NoneNodeStyle.png", typeof(Texture2D));
                    _noneNodeStyle.border = new RectOffset(7, 7, 7, 7);
                    _noneNodeStyle.alignment = TextAnchor.MiddleCenter;
                    _noneNodeStyle.fontStyle = FontStyle.Bold;
                    
                }

                return _noneNodeStyle;
            }
        }

        private static GUIStyle _runningNodeStyle;

        public static GUIStyle runningNodeStyle
        {
            get
            {
                if (_runningNodeStyle == null)
                {
                    _runningNodeStyle = new GUIStyle();
                    _runningNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath
                        ("Assets/EasyBT/Editor/Texture/RunningNodeStyle.png", typeof(Texture2D));
                    _runningNodeStyle.border = new RectOffset(7, 7, 7, 7);
                    _runningNodeStyle.alignment = TextAnchor.MiddleCenter;
                    _runningNodeStyle.fontStyle = FontStyle.Bold;
                    _runningNodeStyle.fontSize = 13;
                }

                return _runningNodeStyle;
            }
        }

        private static GUIStyle _failureNodeStyle;
        public static GUIStyle failureNodeStyle
        {
            get
            {
                if (_failureNodeStyle == null)
                {
                    _failureNodeStyle = new GUIStyle();
                    _failureNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath
                        ("Assets/EasyBT/Editor/Texture/FailureNodeStyle.png", typeof(Texture2D));
                    _failureNodeStyle.border = new RectOffset(7, 7, 7, 7);
                    _failureNodeStyle.alignment = TextAnchor.MiddleCenter;
                    _failureNodeStyle.fontStyle = FontStyle.Bold;
                    _failureNodeStyle.fontSize = 13;
                }

                return _failureNodeStyle;
            }
        }

        private static GUIStyle _successNodeStyle;
        public static GUIStyle successNodeStyle
        {
            get
            {
                if (_successNodeStyle == null)
                {
                    _successNodeStyle = new GUIStyle();
                    _successNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath
                        ("Assets/EasyBT/Editor/Texture/SuccessNodeStyle.png", typeof(Texture2D));
                    _successNodeStyle.border = new RectOffset(7, 7, 7, 7);
                    _successNodeStyle.alignment = TextAnchor.MiddleCenter;
                    _successNodeStyle.fontStyle = FontStyle.Bold;
                    _successNodeStyle.fontSize = 13;
                }

                return _successNodeStyle;
            }
        }

        private static GUIStyle _selectedNodeStyle;
        public static GUIStyle selectedNodeStyle
        {
            get
            {
                if (_selectedNodeStyle == null)
                {
                    _selectedNodeStyle = new GUIStyle();
                    _selectedNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath
                        ("Assets/EasyBT/Editor/Texture/SelectedFrameStyle.png", typeof(Texture2D));
                    _selectedNodeStyle.border = new RectOffset(7, 7, 7, 7);
                }

                return _selectedNodeStyle;
            }
        }
    }
}
