using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    public class Mathc
    {
        public static bool AABB(Rect rectA, Rect rectB)
        {
            //降維，判斷長寬線段是否有交集
            return (Mathf.Min(rectA.x,rectA.x + rectA.width) < rectB.x + rectB.width &&
                Mathf.Max(rectA.x, rectA.x + rectA.width) > rectB.x &&
                Mathf.Min(rectA.y, rectA.y + rectA.height) < rectB.y + rectB.height &&
                Mathf.Max(rectA.y, rectA.y + rectA.height) > rectB.y) == true ? true : false;
        }

        private static float DistanceBetweenPointAndLine(Vector2 p1, Vector2 p2, Vector2 point)
        {
            float a = 0;
            float b = 0;
            float c = 0;
            float distance = 0;

            //平行於y軸 L:x=c
            if (p1.x == p2.x)
            {
                a = 1;
                b = 0;
                c = -p1.x;
                distance = Mathf.Abs(point.x + c);

            }
            //平行於x軸 L:y=c
            else if (p1.y == p2.y)
            {
                c = -p1.y;
                b = 1;
                a = 0;
                distance = Mathf.Abs(point.y + c);

            }
            //y = mx + b
            else
            {
                float m = (p1.y - p2.y) / (p1.x - p2.x);
                a = -m;
                c = -p1.y + m * p1.x;
                b = 1;
                distance = Mathf.Abs(a * point.x + b * point.y + c) / Mathf.Sqrt(Mathf.Pow(a, 2) + 1);
            }
            return distance;
        }

        /// <summary>
        /// Point on a straight line?
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="point"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static bool IsPointHoverStraightLine(Vector2 p1, Vector2 p2, Vector2 point, float tolerableDis)
        {

            float magnitude_P1P2 = (p1 - p2).magnitude;
            float magnitude_P1P = (p1 - point).magnitude;
            float magnitude_P2P = (p2 - point).magnitude;

            if (DistanceBetweenPointAndLine(p1, p2, point) <= tolerableDis &&
                ((magnitude_P1P < magnitude_P1P2 && magnitude_P2P < magnitude_P1P2)))
            {
                return true;
            }

            return false;
        }
    }

}
