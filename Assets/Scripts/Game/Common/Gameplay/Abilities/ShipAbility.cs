using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ship Abilities", fileName = "New Ability", order = 1)]
public abstract class ShipAbility : ScriptableObject {
    [SerializeField] protected float cooldown;
    // The command that triggers execution of this ability
    [SerializeField] protected CommandType executeCommand;
    // The command that triggers this ability to stop
    [SerializeField] protected CommandType stopCommand;
    public CommandType ExecuteCommand => executeCommand;
    public CommandType StopCommand => stopCommand;
    public float Cooldown => cooldown;

    // Optional function for resolving network lag visual issues (especially on coroutine dependent functions)
    public virtual void ExecuteOnce(ShipManager shipManager, bool isServer) { }

    // Execute the ability
    public abstract void Execute(ShipManager shipManager, bool isServer);
    
    // Stop executing the ability - may be optional
    public virtual void Stop(ShipManager shipManager, bool isServer) { }

    // Add the corresponding behaviour to the given game object
    public abstract void AddBehaviour(GameObject gameObject);
}
