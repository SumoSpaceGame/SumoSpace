using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    public class Pylon : MonoBehaviour
    {
        public PylonGraphics graphics;

        public PylonMap map;

        public PylonAnimation pylonAnimation;

        /// <summary>
        /// Used for point list updating
        /// </summary>
        [HideInInspector] public int pylonIndex;

        public Pylon ConnectedTo;

        public void Start()
        {
            pylonAnimation.CurrentGameobject = this;

            if (pylonAnimation.CurrentGameobject != this)
            {
                Debug.LogError("One pylon animation per game object. Instead found multiple using " + pylonAnimation);
            }
        }

        /// <summary>
        /// Gets set by pylon map, or can be set by itself.
        /// Used for updating pylon animation 
        /// Sets the animation position based on current time set;
        /// </summary>
        public void SetAnimationPosition()
        {
            float perc = Mathf.Clamp(map.currentTime / map.maxTime, 0.0f, 1.0f);
            
            var newFrame = new PylonKeyFrame()
            {
                percentage = perc,
                position = new Vector2(this.transform.position.x, this.transform.position.z)
            };
            
            int afterFrame = -1;
            for (int i = 0; i < pylonAnimation.keyFrames.Count; i++)
            {
                var frame = pylonAnimation.keyFrames[i];
                
                if (frame.percentage < perc)
                {
                    afterFrame = i;
                }

                if (Math.Abs(frame.percentage - perc) < float.Epsilon)
                {
                    pylonAnimation.keyFrames[i] = newFrame;
                    return;
                }
            }

            
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
            graphics.pylonA = this;
            graphics.pylonB = ConnectedTo;
            
            if (!pylonAnimation)
            {
                return new Vector2(this.transform.position.x, this.transform.position.z);
            }
            
            Vector2 position = pylonAnimation.GetPosition(percentage);
            this.transform.position = new Vector3(position.x, 0, position.y);
            return position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(this.transform.position, this.ConnectedTo.transform.position);
            Gizmos.DrawWireSphere(this.transform.position, 0.1f);
        }
    }
}
