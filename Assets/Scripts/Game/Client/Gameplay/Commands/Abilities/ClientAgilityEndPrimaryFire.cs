using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class EndGun: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        if (!manager.isPlayer) {
            Debug.Log("Other");
            manager.shipLoadout.PrimaryFire.Stop(manager, false);
        } else {
            Debug.Log("Me");
        }
        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        Debug.Log("Performing");
        shipManager.shipLoadout.PrimaryFire.Stop(shipManager, false);
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_END_WEAPON, shipManager.playerMatchID);
        return true;
    }
}