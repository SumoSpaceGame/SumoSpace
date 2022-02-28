using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ServerAgilityEndPrimaryFire: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.shipLoadout.PrimaryFire.Stop(manager, true);
        networker.SendData(packetData, (int)CommandType.AGILITY_END_WEAPON, manager.playerMatchID);
        return true;
    }
}