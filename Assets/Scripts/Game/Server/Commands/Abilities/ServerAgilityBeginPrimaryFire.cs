using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ServerAgilityBeginPrimaryFire: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.shipLoadout.PrimaryFire.Start(manager, true);
        networker.SendData(packetData, (int)CommandType.AGILITY_START_WEAPON, manager.playerMatchID);
        return true;
    }
}