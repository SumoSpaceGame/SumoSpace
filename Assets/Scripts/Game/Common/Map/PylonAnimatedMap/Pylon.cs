using UnityEngine;

namespace Game.Common.Map.PylonAnimatedMap
{
    public class Pylon : MonoBehaviour
    {
        //Have a bounding box sized around the its connections, so A<->this<->B, will have a collision box of AB

        
        /// <summary>
        /// Current match time to jump too for this interpolation
        /// </summary>
        /// <param name="currentTime"></param>
        public void SetTime(float currentTime)
        {
            
        }

    }
}