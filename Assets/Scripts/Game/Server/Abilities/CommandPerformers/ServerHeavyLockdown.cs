using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ServerHeavyLockdown : ICommand
{
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
    {
        shipManager.shipLoadout.PrimaryFire.Execute(shipManager, true);
        networker.SendData(packetData, CommandType.HEAVY_LOCKDOWN, shipManager.playerMatchID);
        return true;
    }
}