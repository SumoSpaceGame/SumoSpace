using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class StartPrimaryFire: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.StartGun();
        networker.SendData(packetData, (int)CommandType.START_FIRE, manager.playerMatchID);
        return true;
    }
}