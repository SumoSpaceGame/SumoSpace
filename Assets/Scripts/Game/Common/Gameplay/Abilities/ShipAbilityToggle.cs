using Game.Common.Gameplay.Ship;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ship Abilities", fileName = "New Toggle Ability")]
public abstract class ShipAbilityToggle: ShipAbility {
    public override void Execute(ShipManager shipManager, bool isServer = false) {
        Start(shipManager, isServer);
    }

    public abstract void Start(ShipManager shipManager, bool isServer = false);
    public abstract void Stop(ShipManager shipManager, bool isServer = false);
}