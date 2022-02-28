using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities", fileName = "New Loadout",order = 0)]
public class ShipLoadout : ScriptableObject {
    public ShipMovement ShipMovement;
    public ShipAbilityToggle PrimaryFire;
    public ShipAbility PrimaryAbility;
    public ShipAbility SecondaryAbility;
}