using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using UnityEngine;

namespace Game.Ships.Heavy.Common.Abilities
{
    [CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Primary Fire", fileName = "Heavy Primary Fire", order = 1)]
    public class HeavyPrimaryFireAbility : ShipAbility {

        protected override void OnInit()
        {
            this.executeCommand = CommandTypes.COMMAND_HEAVY_PRIMARY_FIRE_START;
            this.stopCommand = CommandTypes.COMMAND_HEAVY_PRIMARY_FIRE_END;
        }
        
        [SerializeField] private float minKnockback;
        /// <summary>
        /// Minimum knockback after applying multipliers.
        /// </summary>
        public float MinKnockback => minKnockback * KnockbackMultiplier;

        [SerializeField] private float maxKnockback;
        /// <summary>
        /// Maximum knockback after applying multipliers.
        /// </summary>
        public float MaxKnockback => maxKnockback * KnockbackMultiplier;

        [SerializeField] private float rampTime;
        public float RampTime => rampTime;
    
        [SerializeField] private AnimationCurve knockbackCurve;
        public AnimationCurve KnockbackCurve => knockbackCurve;

        public float KnockbackMultiplier = 1f;

        /// <summary>
        /// If true, the ship can't fire.
        /// </summary>
        public bool IsDisabled;

        /// <summary>
        /// Returns the current amount of knockback that should be used.
        /// </summary>
        /// <remarks>
        /// Knockback scales linearly until maximum is reached.
        /// </remarks>
        /// <param name="time"> The amount of time the ability has been active.</param>
        /// <returns></returns>
        public float CurrentKnockback(float time)
        {
            float charge = KnockbackCurve.Evaluate(time / RampTime); // Between 0 and 1.
            return Mathf.Lerp(MinKnockback, MaxKnockback, charge);
        }
    }
}
