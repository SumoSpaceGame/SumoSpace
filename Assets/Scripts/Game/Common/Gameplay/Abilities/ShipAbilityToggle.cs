using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ship Abilities", fileName = "New Toggle Ability")]
public abstract class ShipAbilityToggle: ShipAbility {
    [SerializeField] protected CommandType stopCommand;

    public CommandType StopCommand => stopCommand;

    public abstract void Stop(ShipManager shipManager, bool isServer);
}