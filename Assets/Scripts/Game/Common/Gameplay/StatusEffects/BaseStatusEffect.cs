using UnityEngine;

namespace UnityTemplateProjects.Game.Common.Gameplay.StatusEffects
{
    public abstract class BaseStatusEffect : ScriptableObject
    {
        public bool Stackable;
        public int StackLimit;
        public int CurrentCounter;

        public void StartEffect(ref StatusEffectValues values)
        {
            OnStart(ref values);
        }

        public void UpdateEffect(ref StatusEffectValues values)
        {
            OnUpdate(ref values);
        }

        public void StopEffect(ref StatusEffectValues values, bool forced = false)
        {
            OnStop(ref values, forced);
        }
        public abstract void OnStart(ref StatusEffectValues values);
        public abstract void OnUpdate(ref StatusEffectValues values);
        public abstract void OnStop(ref StatusEffectValues values, bool forced = false);

    }
}