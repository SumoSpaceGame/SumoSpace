using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ServerHeavyBurst : ICommand
{
    public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
    {
        shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, true);
        networker.SendData(packetData, CommandType.HEAVY_BURST, shipManager.playerMatchID);
        return true;
    }
}