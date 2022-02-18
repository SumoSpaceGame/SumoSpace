using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class EndGun: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        if (manager.isPlayer) { // Client
            manager.simulationObject.representative.GetComponent<ShipRenderer>().StopShoot();
            Debug.Log("I'm no longer shooting");
        } else { 
            manager.simulationObject.representative.GetComponent<ShipRenderer>().StopShoot();
            Debug.Log("They're no longer shooting");
        }

        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        Debug.Log("Ceasing Fire");
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.END_FIRE, shipManager.playerMatchID);
        return true;
    }
}