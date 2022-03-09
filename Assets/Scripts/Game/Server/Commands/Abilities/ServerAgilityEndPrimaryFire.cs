using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ServerAgilityEndPrimaryFire: ICommand {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Stop(shipManager, true);
        networker.SendData(packetData, (int)CommandType.AGILITY_END_WEAPON, shipManager.playerMatchID);
        return true;
    }
}