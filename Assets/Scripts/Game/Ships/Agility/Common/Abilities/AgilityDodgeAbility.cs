using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using UnityEngine;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Dodge", fileName = "Agility Dodge", order=1)]
    public class AgilityDodgeAbility: ShipAbility
    {
        protected override void OnInit()
        {
            this.executeCommand = CommandTypes.COMMAND_AGILITY_DODGE;
        }
        
        [SerializeField] private float distance;
        public float Distance => distance;
        
        [SerializeField] private float time;
        public float Time => time;
        
        [SerializeField] private float speed;
        public float Speed => speed;
    }
}