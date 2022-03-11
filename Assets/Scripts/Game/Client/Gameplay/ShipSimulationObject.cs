using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ShipSimulationObject : SimulationObject {
    private ShipManager shipManager;

    public void Create(ShipManager sm) {
        shipManager = sm;
        Create();
    }
}