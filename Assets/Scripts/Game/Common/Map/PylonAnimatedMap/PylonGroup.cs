using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map.PylonAnimatedMap
{
    public class PylonGroup : MonoBehaviour
    {
        
        public List<Pylon> pylons = new List<Pylon>();

        /// <summary>
        /// Defines if this is a safe zone for the player, aka if the player is safe within the map or not.
        ///
        /// Precedence should be false over true
        /// </summary>
        public bool SafeZone = false;
        
        public void SetTime(float currentTime)
        {
            foreach (var pylon in pylons)
            {
                pylon.SetTime(0);
            }
        }

        public void WithinZone(Vector2 point)
        {
            
        }

        public void WithinZone(Vector2 point, int radius)
        {
            
        }
    }
}