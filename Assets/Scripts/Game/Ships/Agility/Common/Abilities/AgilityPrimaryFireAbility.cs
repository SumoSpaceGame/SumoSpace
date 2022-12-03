using Game.Common.Gameplay.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Common.Abilities
{
    [CreateAssetMenu(menuName="Ship Abilities/Agility Abilities", fileName = "Agility Primary Fire", order=1)]
    public class AgilityPrimaryFireAbility: ShipAbility {

        [SerializeField] private float knockback;
        public float Knockback => knockback;
    }
}
