using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Micro Missile", fileName = "Agility Micro Missile", order=1)]
    public class AgilityMicroMissileAbility: ShipAbility
    {
        [SerializeField] private float maxMissileTime;
        public float MaxMissileTime => maxMissileTime;

        [SerializeField] private float acceleration;
        public float Acceleration => acceleration;
        
        [SerializeField] private float maxSpeed;
        public float MaxSpeed => maxSpeed;
        
        [SerializeField] private float numMissiles;
        public float NumMissiles => numMissiles;
        
        [SerializeField] private float attackDuration;
        public float AttackDuration => attackDuration;
        
        [SerializeField] private float explosionRadius;
        public float ExplosionRadius => explosionRadius;
        
        [SerializeField] private float explosionForce;
        public float ExplosionForce => explosionForce;
    }
}
