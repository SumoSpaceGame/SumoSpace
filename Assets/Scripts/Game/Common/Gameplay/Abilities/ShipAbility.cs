using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ship Abilities", fileName = "New Ability", order = 1)]
public abstract class ShipAbility: ScriptableObject {
    [SerializeField] protected float cooldown;
    [SerializeField] protected CommandType command;

    public CommandType Command => command;
    public float Cooldown => cooldown;

    public abstract void Execute(ShipManager shipManager, bool isServer = false);
}
