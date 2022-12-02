using UnityEngine;

namespace Game.Common.Map
{
    /// <summary>
    /// Boundaries are things not defined by the game map itself, and are standalone
    ///
    /// For example a circle on the map
    /// </summary>
    public abstract class GameMapBoundary : MonoBehaviour
    {
        /// <summary>
        /// Ranked SAFE, WARNING, DEAD
        /// SAFE > WARNING > DEAD
        /// </summary>
        public MapTrackerBase.CurrentLayer type;
        
        /// <summary>
        /// Less than 0 for map priority first (For example a deadzone that appears when the map closes
        /// </summary>
        public int Priority;

        public abstract bool IsWithin(Vector2 pos);
        public abstract bool IsWithin(Vector2 pos, float radius);

        public abstract MapTrackerBase.CurrentLayer GetLayer(Vector2 pos);
        public abstract MapTrackerBase.CurrentLayer GetLayer(Vector2 pos, float radius);

    }
}