using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Map
{
    public class ShipMapTracker : MapTrackerBase
    {
        public ShipManager manager;
        
        public override Vector2 GetPosition()
        {
            return manager.Get2DPosition();
        }

        public override float GetRadius()
        {
            return manager.GetRadius();
        }

        public override void OnLayerChange()
        {
            // Do nothing yet
            // If player do something?
        }
    }
}