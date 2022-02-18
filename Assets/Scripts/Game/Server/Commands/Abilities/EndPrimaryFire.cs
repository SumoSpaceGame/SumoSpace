using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class EndPrimaryFire: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.StopGun();
        networker.SendData(packetData, (int)CommandType.END_FIRE, manager.playerMatchID);
        return true;
    }
}