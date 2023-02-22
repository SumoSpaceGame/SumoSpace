using Game.Common.Gameplay.Ship;

public class ShipSimulationObject : SimulationObject {
    private ShipManager shipManager;

    public void Create(ShipManager sm) {
        shipManager = sm;
        Create();
    }
    
    
}