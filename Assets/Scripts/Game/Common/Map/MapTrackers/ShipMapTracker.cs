using System;
using Game.Common.Gameplay.Ship;
using Game.Common.ScriptableData;
using UnityEngine;

namespace Game.Common.Map
{
    public class ShipMapTracker : MapTrackerBase
    {
        public ShipManager manager;

        public FloatScriptableData shipWarningSeconds;
        
        [SerializeField]
        private float currentWarningTimer;
        
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
            
        }

        private void Update()
        {
            bool isDead = false;
            
            switch (this.currentLayer)
            {
                case CurrentLayer.SAFE:
                {
                    currentWarningTimer = 0;
                    break;
                }
                case CurrentLayer.WARNING:
                {
                    currentWarningTimer += Time.deltaTime;

                    if (currentWarningTimer >= shipWarningSeconds.value)
                    {
                        isDead = true;
                    }
                    break;                    
                }
                case CurrentLayer.DEAD:
                {
                    isDead = true;
                    break;
                }
            }

            if (isDead)
            {
                manager.Kill();
            }
        }
    }
}