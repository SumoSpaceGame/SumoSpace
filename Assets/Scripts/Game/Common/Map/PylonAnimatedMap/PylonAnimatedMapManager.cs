using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map.PylonAnimatedMap
{
    public class PylonAnimatedMapManager : MonoBehaviour, IGameMap
    {
        public List<PylonGroup> pylonGroups = new List<PylonGroup>();
        
        //Take pylons
        //Update them with the current time, they will interpolate themselves and move themselves
        
        //Pylon Collision Check
        //Use PylonCollsion to check which pylons to collide against
        //Create bounding boxes within the pylons to 


        private void Update()
        {
            //Update the pylons
            foreach (var group in pylonGroups)
            {
                group.SetTime(0);
            }
        }

        public bool WithinMap(Vector2 point)
        {
            return false;
        }

        public bool WithinMap(Vector2 point, float radius)
        {
            return false;
        }
    }
}