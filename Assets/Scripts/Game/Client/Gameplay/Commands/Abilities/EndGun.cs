using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class EndGun: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        if (manager.isPlayer) { // Client
            //manager.simulationObject.representative.GetComponent<ShipRenderer>().StopShoot();
        } else { 
            manager.simulationObject.representative.GetComponent<ShipRenderer>().StopShoot();
        }

        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        Debug.Log("Ceasing Fire");
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_END_WEAPON, shipManager.playerMatchID);
        return true;
    }
}