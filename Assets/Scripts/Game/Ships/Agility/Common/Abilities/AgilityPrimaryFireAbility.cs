using Game.Common.Gameplay.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities/Agility Abilities/Agility Primary Fire", fileName = "Agility Primary Fire", order=1)]
public class AgilityPrimaryFireAbility: ShipAbility {

    [SerializeField] private float knockback;
    public float Knockback => knockback;
}
