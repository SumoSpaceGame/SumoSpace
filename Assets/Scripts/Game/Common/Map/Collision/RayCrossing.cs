using UnityEngine;
using UnityEngine.Analytics;

namespace Game.Common.Map.Collision
{

    public struct CircularPointList
    {
        // Points are devised into a circular list
        // 0 -> 1 -> 2 -> 0
        public Vector2[] points;
    }
    public class RayCrossing : MonoBehaviour
    {
        public bool PointInside(ref CircularPointList list, Vector2 point)
        {
            if (list.points.Length == 0)
            {
                Debug.LogError("Passed in an empty list for RayCrossing. Defaulting False");
                return false;
            }
            
            Vector2 start = point;
            Vector2 end = point + Vector2.right * Mathf.Infinity;

            int intersectionCount = 0;
            Vector2 pointA, pointB;
            
            for (int i = 0; i < list.points.Length; i++)
            {
                pointA = list.points[i];
                pointB = i +1 == list.points.Length ? list.points[0] : list.points[i + 1];
                
                //Check if horizontal, if it is ignore this line.

                if (Mathf.Approximately(pointA.y, pointB.y))
                {
                    continue;
                }

                if (LineIntersect(start, end, pointA, pointB))
                {
                    intersectionCount += 1;
                }
            }
            
            // Even = OUT, Odd = IN
            // Checks if it is Odd
            return intersectionCount % 2 != 0;
        }
        
        
        public bool LineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 e = c - a;
            Vector2 r = d - b;
            Vector2 s = d - c;

            float eXr = e.x * r.y - e.y * r.x;
            float eXs = e.x * s.y - e.y * s.x;
            float rXs = r.x * s.y - r.y * s.x;

            if (Mathf.Approximately(eXr, 0))
            {
                // Lines are collinear, intersect if any overlap.
                return ((c.x - a.x < 0) != (c.x - b.x < 0))
                       || ((c.y - a.y < 0) != (c.y - b.y < 0));
            }

            if (Mathf.Approximately(rXs , 0f))
            {
                return false; // Parallel
            }

            float rxsr = 1f / rXs;
            float t = eXs * rxsr;
            float u = eXr * rxsr;

            return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
        }
    }
}
