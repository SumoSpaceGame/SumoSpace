using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ClientAgilityBeginPrimaryFire: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.shipLoadout.PrimaryFire.Execute(manager, false);
        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_START_WEAPON, shipManager.playerMatchID);
        return true;
    }
}