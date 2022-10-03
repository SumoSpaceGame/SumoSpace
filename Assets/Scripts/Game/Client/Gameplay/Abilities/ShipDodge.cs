using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ShipDodge: ICommandPerformer {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.simulationObject.representative.GetComponent<Animator>().SetTrigger("Dodge");
        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        Debug.Log("Dodge triggered");
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_DODGE, shipManager.playerMatchID);
        return true;
    }
}