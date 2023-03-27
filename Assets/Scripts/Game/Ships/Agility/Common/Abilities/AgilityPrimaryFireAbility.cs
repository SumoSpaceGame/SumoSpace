using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using UnityEngine;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Primary Fire", fileName = "Agility Primary Fire", order=1)]
    public class AgilityPrimaryFireAbility: ShipAbility {

        protected override void OnInit()
        {
            this.executeCommand = CommandTypes.COMMAND_AGILITY_PRIMARY_FIRE_START;
            this.stopCommand = CommandTypes.COMMAND_AGILITY_PRIMARY_FIRE_END;
        }
        
        [SerializeField] private float knockback;
        public float Knockback => knockback;
    }
}
