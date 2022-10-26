using UnityEngine;

namespace Game.Common.Gameplay.Abilities.Agility
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Dodge", fileName = "Agility Dodge", order=1)]
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