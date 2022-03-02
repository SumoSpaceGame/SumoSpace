using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ClientAgilityBeginPrimaryFire: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.shipLoadout.PrimaryFire.Execute(manager, false);
        //if (manager.isPlayer) { // This client -- ignore
            //manager.simulationObject.representative.GetComponent<ShipRenderer>().Shoot();
            //Debug.Log("Im shooting");
        //} else {
            //manager.simulationObject.representative.GetComponent<ShipRenderer>().Shoot();
            //Debug.Log("Theyre shooting");
        //}

        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        //Debug.Log("Starting fire");
        shipManager.shipLoadout.PrimaryFire.Execute(shipManager, false);
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_START_WEAPON, shipManager.playerMatchID);
        return true;
    }
}