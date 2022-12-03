using Game.Common.Gameplay.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities", fileName = "Agility Dodge", order=1)]
    public class AgilityDodgeAbility: ShipAbility
    {
        [SerializeField] private float distance;
        public float Distance => distance;
        
        [SerializeField] private float time;
        public float Time => time;
        
        [SerializeField] private float speed;
        public float Speed => speed;
    }
}