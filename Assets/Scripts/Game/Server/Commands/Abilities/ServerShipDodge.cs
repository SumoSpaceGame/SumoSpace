using System.Collections;
using System.Data.Common;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ServerShipDodge: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.Dodge();
        networker.SendData(packetData, (int)CommandType.AGILITY_DODGE, manager.playerMatchID);
        return true;
    }
}