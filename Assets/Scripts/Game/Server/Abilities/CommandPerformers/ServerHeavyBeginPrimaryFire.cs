using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ServerHeavyBeginPrimaryFire: ICommand {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Execute(shipManager, true);
        networker.SendData(packetData, (int)CommandType.HEAVY_PRIMARY_FIRE_START, shipManager.playerMatchID);
        return true;
    }
}