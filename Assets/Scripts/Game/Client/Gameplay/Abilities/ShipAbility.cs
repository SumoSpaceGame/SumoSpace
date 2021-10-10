using UnityEngine;

public abstract class ShipAbility : ScriptableObject, IAbility {
    private int ID;
    public Ship ship; // Ship this ability is bound to

    /*
     * If the ability is performed on a Player Ship, send this data over the server so everyone else sees it
     * Then, render out this ability regardless on the ship this is attached to
     */
    public void Perform() {
        if(ship.isPlayer) Execute();
        Render();
    }
    
    protected abstract void Execute();
    
    protected abstract void Render();

    //Command Handler
    //Dictionary<id, abilityClass>

    //Server & Client functions (Separate classes based on an interface)
    //public abstract void Receive(string data);

    public void Receive(string data) {
        throw new System.NotImplementedException();
    }
}
