using UnityEngine;

namespace Game.Common.Map
{
    public interface IGameMap
    {
        /// <summary>
        /// Checks if a point is in the map
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool WithinMap(Vector2 point);
    
        /// <summary>
        /// Checks if a sphere is within the map
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool WithinMap(Vector2 point, float radius);
    
    
    }
}
