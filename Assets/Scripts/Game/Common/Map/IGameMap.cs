using UnityEngine;

namespace Game.Common.Map
{
    public interface IGameMap
    {

        /// <summary>
        /// Called on start before a map is used/updated
        /// </summary>
        /// <param name="build">Makes sure no scene objects get made</param>
        public void Init(bool build = true);

        /// <summary>
        /// Updates the map with the current matchTime. Delta time should be handled by the map itself
        /// </summary>
        /// <param name="matchTime">Current Match time</param>
        public void UpdateMap(double matchTime);
        

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
        
        /// <summary>
        /// Checks if a sphere is within the map
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool OnEdge(Vector2 point, float radius);
    
    
    }
}
