using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ServerAgilityBeginPrimaryFire: ICommand {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Execute(shipManager, true);
        networker.SendData(packetData, (int)CommandType.AGILITY_START_WEAPON, shipManager.playerMatchID);
        return true;
    }
}