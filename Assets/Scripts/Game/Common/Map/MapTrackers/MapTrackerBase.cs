using System;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Map
{
    public abstract class MapTrackerBase : MonoBehaviour
    {
        public enum CurrentLayer
        {
            SAFE = 0, WARNING = 1, DEAD = 2
        }
        
        private CurrentLayer _currentLayer;
        
        public CurrentLayer currentLayer
        {
            get => _currentLayer;
            set
            {
                if (_currentLayer != value)
                {
                    OnLayerChange();
                    _currentLayer = value;
                }
            }
        }

        private void Start()
        {
            MainInstances.Get<GameMapManager>().mapTrackers.Add(this);
        }

        private void OnDestroy()
        {
            MainInstances.Get<GameMapManager>().mapTrackers.Remove(this);
        }
        
        /// <summary>
        /// Greatest is determined by the current enum value and the one provided;
        /// </summary>
        /// <param name="layer"></param>
        public void SetGreatest(CurrentLayer layer)
        {
            if (currentLayer < layer)
            {
                currentLayer = layer;
            }
        }

        public abstract Vector2 GetPosition();
        public abstract float GetRadius();
        public abstract void OnLayerChange();
        


    }
}