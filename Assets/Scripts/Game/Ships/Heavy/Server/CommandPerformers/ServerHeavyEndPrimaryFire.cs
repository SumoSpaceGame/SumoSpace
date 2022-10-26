using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ServerHeavyEndPrimaryFire: ICommand {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Stop(shipManager, true);
        networker.SendData(packetData, CommandType.HEAVY_PRIMARY_FIRE_END, shipManager.playerMatchID);
        return true;
    }
}