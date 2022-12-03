using UnityEngine;

namespace UnityTemplateProjects.Game.Common.Gameplay.StatusEffects
{
    public class TimedStatusEffect : BaseStatusEffect
    {
        public float currentTime;
        public float LengthOfTime;
        public bool RestartTimerOnStack = false;
        /// <summary>
        /// Clears the stack when the timer is finished, otherwise just subtracts one
        /// </summary>
        public bool ClearStackOnTimer = false;
        
        public override void OnStart(ref StatusEffectValues values)
        {
            if (values.counter == 0 || RestartTimerOnStack)
            {
                values.time = 0;
            }
            values.counter += 1;
        }

        public override void OnUpdate(ref StatusEffectValues values)
        {
            values.time += Time.deltaTime;

            if (currentTime > LengthOfTime)
            {
                if (ClearStackOnTimer)
                {
                    StopEffect(ref values);
                    
                }else if (values.counter > 1)
                {
                    values.counter -= 1;
                    values.time = 0;
                }
                else
                {
                    StopEffect(ref values);
                }
            }
        }

        public override void OnStop(ref StatusEffectValues values, bool forced = false)
        {
            values.counter = 0;
            values.time = 0;
        }
    }
}