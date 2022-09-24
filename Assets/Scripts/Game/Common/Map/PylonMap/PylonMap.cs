using System;
using System.Collections.Generic;
using Game.Common.Map.Collision;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Common.Map.PylonMap
{
    public class PylonMap : MonoBehaviour, IGameMap
    {
        public List<Pylon> pylons = new List<Pylon>();
        public PointList pointList;

        public float currentTime = 0;
        public float maxTime = 0;

        [HideInInspector] public bool initialized = false;

        public PylonBuilder builder;
        
        /// <summary>
        /// Create the point list and connections needed
        /// </summary>
        public void Init()
        {
            
            List<Vector2> points = new List<Vector2>();
            
            // TODO - Just add all the pylons in the scene, find objects.
            foreach (var pylon in pylons)
            {
                points.Add(new Vector2(pylon.transform.position.x, pylon.transform.position.y));
                pylon.pylonIndex = points.Count - 1;
            }

            List<int> connections = new List<int>();
            for (int i = 0; i < pylons.Count; i++)
            {
                connections.Add(i);
                connections.Add(pylons[i].ConnectedTo.pylonIndex);
            }

            pointList.connections = connections.ToArray();
            pointList.points = points.ToArray();

            initialized = true;

            builder.Build(this);
        }

        
        /// <summary>
        /// Update the map
        /// </summary>
        /// <param name="matchTime"></param>
        public void UpdateMap(float matchTime)
        {
            currentTime = matchTime;
            float perc = currentTime / maxTime;
            
            for (int i = 0; i < pylons.Count; i++)
            {
                pointList.points[pylons[i].pylonIndex] = pylons[i].UpdatePosition(perc);
                // TODO: If point removal system is added, have it destroy the point here if it has been destroyed
            }
            
            builder.UpdateGraphics();
        }
        
        /// <summary>
        /// Checks if point is within map
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool WithinMap(Vector2 point)
        {
            return RayCrossing.PointInside(ref pointList, point);
        }
        
        /// <summary>
        /// Checks if the ship is within the map
        /// Might not be too accurate, since it goes by 4 points
        /// Would maybe be better to use a single point
        /// Or 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool WithinMap(Vector2 point, float radius)
        {
            bool inside = RayCrossing.PointInside(ref pointList, point);

            if (inside)
            {
                return true;
            }
            
            // Within Map if the closestPoint is less than radius
            return RayCrossing.ClosestPointDistance(ref pointList, point) < radius;
        }

        public bool OnEdge(Vector2 point, float radius)
        {
            return RayCrossing.ClosestPointDistance(ref pointList, point) < radius;
        }


        private void OnDrawGizmosSelected()
        {
            
            Gizmos.color = Color.yellow;
            if (pointList.connections == null || pointList.connections.Length == 0) return;
            
            for (int i = 0; i < pointList.connections.Length; i += 2)
            {
                Vector3 position = new Vector3(pointList.points[pointList.connections[i]].x, 0,
                    pointList.points[pointList.connections[i]].y);
                Vector3 position2 = new Vector3(pointList.points[pointList.connections[i + 1]].x, 0,
                    pointList.points[pointList.connections[i + 1]].y);
                
                position += Vector3.down;
                position2 += Vector3.down;
                Gizmos.DrawLine(position, position2);
            }
        }
    }
}
