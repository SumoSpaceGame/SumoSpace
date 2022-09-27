using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ClientHeavyEndPrimaryFire: ICommandPerformer {
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
        shipManager.shipLoadout.PrimaryFire.Stop(shipManager, false);
        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        networker.SendData(CommandPacketData.Create(new byte[]{}), CommandType.HEAVY_PRIMARY_FIRE_END, shipManager.playerMatchID);
        return true;
    }
}