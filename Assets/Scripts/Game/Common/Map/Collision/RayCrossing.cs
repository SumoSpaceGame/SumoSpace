using UnityEngine;
using UnityEngine.Analytics;

namespace Game.Common.Map.Collision
{

    public struct PointList
    {
        public Vector2[] points;
        
        // Every 2 values is a connection between to indexes
        // 0,1 = 0 and 1 are connected
        public int[] connections;
    }
    public class RayCrossing : MonoBehaviour
    {  
        /// <summary>
        /// Checks if the point is contained inside the circular point list
        /// </summary>
        /// <param name="list">List of points, length greater than 1</param>
        /// <param name="point">Point to check if inside or not</param>
        /// <returns></returns>
        public static bool PointInside(ref PointList list, Vector2 point)
        {
            
            Vector2 start = point;
            Vector2 end = point + (Vector2.right * 1000000f);

            int intersectionCount = 0;
            Vector2 pointA, pointB;
            
#if UNITY_EDITOR
            if (list.connections.Length % 2 == 1)
            {
                Debug.LogError("Point List has uneven amount of connections");
                return false;
            }
#endif
            // Things break if list is not even
            for (int i = 0; i < list.connections.Length; i += 2)
            {
                
                pointA = list.points[list.connections[i]];
                pointB = list.points[list.connections[i + 1]];
                
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
        
        /// <summary>
        /// Checks AB against CD to see if they intersect
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool LineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
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
        
        /// <summary>
        /// https://math.stackexchange.com/questions/4079605/how-to-find-closest-point-to-polygon-shape-from-any-coordinate
        /// 
        /// Could use the above to optimize, but for now we do not need to 
        /// </summary>
        /// <param name="pointList"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float ClosestPointDistance(ref PointList pointList, Vector2 point)
        {
            Vector2 pointA, pointB;

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < pointList.connections.Length; i += 2)
            {
                pointA = pointList.points[pointList.connections[i]];
                pointB = pointList.points[pointList.connections[i + 1]];

                Vector2 AP = point - pointA;
                Vector2 AB = pointB - pointA;

                float magnitudeAB = AB.sqrMagnitude;
                float ABAPproduct = Vector2.Dot(AP, AB);
                float distance = ABAPproduct / magnitudeAB;
                float realDist = 0;
                if (distance < 0)
                {
                    realDist = Vector2.Distance(pointA, point);
                }
                else if (distance > 1)
                {
                    realDist = Vector2.Distance(pointB, point);
                }
                else
                {
                    realDist = Vector2.Distance(pointA + AB * distance, point);
                }
                
                if (realDist < closestDistance)
                {
                    closestDistance = realDist;
                }
            }

            return closestDistance;
        }
    }
}
