using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class EndGun: ICommandPerformer {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Stop(shipManager, false);
        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        //shipManager.shipLoadout.PrimaryFire.Stop(shipManager, false);
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_END_WEAPON, shipManager.playerMatchID);
        return true;
    }
}