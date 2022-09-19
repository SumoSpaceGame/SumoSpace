using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    public class Pylon : MonoBehaviour
    {

        public PylonMap map;

        public PylonAnimation pylonAnimation;

        /// <summary>
        /// Used for point list updating
        /// </summary>
        [HideInInspector] public int pylonIndex;

        public Pylon ConnectedTo;
        
        /// <summary>
        /// Gets set by pylon map, or can be set by itself.
        /// Used for updating pylon animation 
        /// Sets the animation position based on current time set;
        /// </summary>
        public void SetAnimationPosition()
        {
            float perc = Mathf.Clamp(map.currentTime / map.maxTime, 0.0f, 1.0f);
            
            int afterFrame = -1;
            for (int i = 0; i < pylonAnimation.keyFrames.Count; i++)
            {
                var frame = pylonAnimation.keyFrames[i];
                
                if (frame.percentage < perc)
                {
                    afterFrame = i;
                }
            }

            var newFrame = new PylonKeyFrame()
            {
                percentage = perc,
                position = new Vector2(this.transform.position.x, this.transform.position.z)
            };
            
            if (afterFrame == -1)
            {
                pylonAnimation.keyFrames.Add(newFrame);

                return;
            }
            
            //Insert it into the list
            List<PylonKeyFrame> newKeyFrames = new List<PylonKeyFrame>();
            for (int i = 0; i < pylonAnimation.keyFrames.Count; i++)
            {
                newKeyFrames.Add(pylonAnimation.keyFrames[i]);
                if (i == afterFrame)
                {
                    newKeyFrames.Add(newFrame);
                }
            }
        }

        public Vector2 UpdatePosition(float percentage)
        {
            Vector2 position = pylonAnimation.GetPosition(percentage);
            this.transform.position = new Vector3(position.x, 0, position.y);
            return position;
        }
    }
}
