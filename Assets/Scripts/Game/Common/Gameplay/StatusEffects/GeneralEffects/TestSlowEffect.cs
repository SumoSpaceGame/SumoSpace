using UnityEngine;

namespace UnityTemplateProjects.Game.Common.Gameplay.StatusEffects.GeneralEffects
{
    [CreateAssetMenu(fileName = "Test Slow Effect", menuName = "Status Effect/Test/Slow Effect")]
    public class TestSlowEffect : TimedStatusEffect
    {
        public override void OnStart(ref StatusEffectValues values)
        {
            base.OnStart(ref values);
        }

        public override void OnUpdate(ref StatusEffectValues values)
        {
            base.OnUpdate(ref values);
        }

        public override void OnStop(ref StatusEffectValues values, bool forced = false)
        {
            base.OnStop(ref values, forced);
        }
    }
}