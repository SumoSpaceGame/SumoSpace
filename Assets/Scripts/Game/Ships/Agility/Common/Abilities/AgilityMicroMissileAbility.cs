using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Micro Missile", fileName = "Agility Micro Missile", order=1)]
    public class AgilityMicroMissileAbility: ShipAbility
    {
        [SerializeField] private float maxMissileTravelDistance;
        public float MaxMissileTravelDistance => maxMissileTravelDistance;
        
        [SerializeField] private float homingAngle;
        public float HomingAngle => homingAngle;
        
        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;
        
        [SerializeField] private float maxSpeed;
        public float MaxSpeed => maxSpeed;
    }
}
