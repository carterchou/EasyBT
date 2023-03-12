using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Editor
{
    public class EditorZoomArea
    {
        public static Rect Begin(float zoomScale, Rect zoomArea)
        {
            GUI.EndGroup();
            Rect clippedArea = zoomArea;
            //beginGroup is used for clipping a vertex  outside the zoomArea
            clippedArea.size *= 87;            
            GUI.BeginGroup(clippedArea);

            Matrix4x4 translation = Matrix4x4.TRS(new Vector3(0, zoomArea.y, 0), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse;            
            return clippedArea;
            
        }

        public static void End()
        {
            GUI.matrix = Matrix4x4.identity;           
        }
    }
}
