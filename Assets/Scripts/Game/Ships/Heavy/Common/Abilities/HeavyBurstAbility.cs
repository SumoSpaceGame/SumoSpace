using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Ships.Heavy.Common.Abilities
{
    [CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Burst", fileName = "Heavy Burst", order = 3)]
    public class HeavyBurstAbility : ShipAbility
    {
        protected override void OnInit()
        {
            this.executeCommand = CommandTypes.COMMAND_HEAVY_BURST;
        }
        
        [Tooltip("How much knockback is needed for one unit of charge")]
        [SerializeField] private float _knockbackToCharge;
        [Tooltip("Energy of burst at minimum charge.")]
        [SerializeField] private float _minEnergy;
        [Tooltip("Energy of burst at full charge.")]
        [SerializeField] private float _maxEnergy;
        [Tooltip("Minimum force the burst will apply.")]
        [SerializeField] private float _minImpulse;
        [Tooltip("Minimum radius that will be used in calculations to prevent small distances creating giant forces.")]
        [SerializeField] private float _minRadius;

        /// <summary>
        /// How much knockback is needed for one unit of charge
        /// </summary>
        public float KnockbackToCharge => _knockbackToCharge;
        /// <summary>
        /// The minimum charge at which the ability can be used.
        /// </summary>
        public ushort MinCharge => (ushort)(_minEnergy * ushort.MaxValue / _maxEnergy);

        /// <summary>
        /// Calculates the maximum radius of the burst.
        /// </summary>
        /// <param name="charge"> The charge of the burst. </param>
        public float MaxRadius(ushort charge) => Energy(charge) / (2 * Mathf.PI * _minImpulse);

        /// <summary>
        /// Calculates the Inpulse applied by the burst.
        /// </summary>
        /// <param name="charge"> The charge of the burst. </param>
        /// <param name="radius"> The distance between the heavy ship and the ship receiving the force. </param>
        public float Impluse(ushort charge, float radius)
        {
            radius = radius < _minRadius ? _minRadius : radius;
            return radius > MaxRadius(charge) ? 0 : Energy(charge) / (2 * Mathf.PI * radius);
        }

        // Calculates the energy of the burst at the desired charge. Returns 0 if there isn't enough charge.
        private float Energy(ushort charge) => charge < MinCharge ? 0 : _maxEnergy / ushort.MaxValue * charge;
    }
}
