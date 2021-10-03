using UnityEngine;

public abstract class ShipAbility : ScriptableObject {
    public Ship ship;
    public abstract void Perform();
}
