using UnityEngine;

namespace Game.Common.Map
{
    public class CircleBoundary : GameMapBoundary
    {
        public float Radius;
        public MapTrackerBase.CurrentLayer layer;

        private Vector2 Get2DPosition()
        {
            return new Vector2(this.transform.position.x, this.transform.position.z);
        }
        public override bool IsWithin(Vector2 pos)
        {
            return ((Get2DPosition() - pos).sqrMagnitude <= Radius * Radius);
        }

        public override bool IsWithin(Vector2 pos, float eleRadius)
        {
            float tempRadius = Radius + eleRadius;
            
            tempRadius *= tempRadius;
            
            return ((Get2DPosition() - pos).sqrMagnitude <= tempRadius);
        }

        public override MapTrackerBase.CurrentLayer GetLayer(Vector2 pos)
        {
            return layer;
        }

        public override MapTrackerBase.CurrentLayer GetLayer(Vector2 pos, float radius)
        {
            return layer;
        }
    }
}