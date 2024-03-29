using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    [Serializable]
    public struct PylonKeyFrame
    {
        public double percentage;
        public Vector2 position;
    }
    
    [CreateAssetMenu(fileName = "Pylon Animation", menuName = "Map/Pylon Animation")]
    public class PylonAnimation : ScriptableObject
    {
        // TODO : Add the ability for the pylon to disappear
        
        public List<PylonKeyFrame> keyFrames = new List<PylonKeyFrame>();

        public Pylon CurrentGameobject;

        
        /// <summary>
        /// Gets the position based on a percentage (0-1)
        /// </summary>
        /// <param name="percentage"></param>
        public Vector2 GetPosition(double percentage)
        {
            if (keyFrames.Count == 0)
            {
                return CurrentGameobject.transform.position;
            }
            
            int frameStart = -1, frameEnd = -1;
            for (int i = 0; i < keyFrames.Count; i++)
            {
                
                if (keyFrames[i].percentage > percentage)
                {
                    frameEnd = i;
                    break;
                }

                frameStart = i;
            }

            if (frameEnd == -1)
            {
                return keyFrames[keyFrames.Count - 1].position;
            }
            else if (frameStart == -1)
            {
                return keyFrames[frameEnd].position;
            }
            
            double remaining = percentage - keyFrames[frameStart].percentage;
            double total = keyFrames[frameEnd].percentage - keyFrames[frameStart].percentage;


            return Vector2.Lerp( keyFrames[frameStart].position, keyFrames[frameEnd].position, (float)(remaining / total));
        }
    }
}
