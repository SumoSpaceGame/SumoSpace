using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using UnityEngine;

namespace Game.Ships.Heavy.Common.Abilities
{
    [CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Lockdown", fileName = "Heavy Lockdown", order = 2)]
    public class HeavyLockdownAbility : ShipAbility
    {
        protected override void OnInit()
        {
            this.executeCommand = CommandTypes.COMMAND_HEAVY_LOCKDOWN;
        }

        [Tooltip("Multiplier applied to the Heavy Primary Fire during Lockdown")]
        [SerializeField] private float _knockbackMultiplier;
        [Tooltip("Multiplier applied to the ship's max force.")]
        [SerializeField] private float _forceMultiplier;
        [Tooltip("Time it takes for ability to activate.")]
        [SerializeField] private float _windUpTime;
        [Tooltip("Time it takes for ability to deactivate.")]
        [SerializeField] private float _windDownTime;

        /// <summary>
        /// Multiplier applied to the Heavy Primary Fire.
        /// </summary>
        public float KnockbackMultiplier => _knockbackMultiplier;

        /// <summary>
        /// Multiplier applied to the ship's max force.
        /// </summary>
        public float ForceMultiplier => _forceMultiplier;
        /// <summary>
        /// Amount of time it takes for the ability to activate. During this time, the player loses control of the ship.
        /// </summary>
        public float WindUpTime => _windUpTime;
        /// <summary>
        /// Amount of time it takes for the ability to deactivate. During this time, the player loses control of the ship.
        /// </summary>
        public float WindDownTime => _windDownTime;
    }
}
