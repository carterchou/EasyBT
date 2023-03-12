using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EasyBT.Runtime;


namespace EasyBT.Editor
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : UnityEditor.Editor
    {
        GUIStyle button = new GUIStyle();

        private void OnEnable()
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.gray);
            texture.Apply();

            button.normal.background = texture;
            button.fontSize = 15;
            button.alignment = TextAnchor.MiddleCenter;
        }

        public override void OnInspectorGUI()
        {
           
            base.OnInspectorGUI();

            if(GUILayout.Button("Youtube",button,GUILayout.Height(30)))
            {
                Application.OpenURL("https://www.youtube.com/channel/UC5r6PlbJx-N8DIb0FANx39w");
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Document",button, GUILayout.Height(30)))
            {
                Application.OpenURL("https://github.com/FierceMalaymo/EasyBT");
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Open"))
            {
                DesignerWindow.Init();
            }
        }
    }
}