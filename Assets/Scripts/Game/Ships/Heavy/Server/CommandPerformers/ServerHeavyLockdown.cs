using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Server.CommandPerformers
{
    public class ServerHeavyLockdown : ICommand
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, true);
            networker.SendData(packetData, CommandType.HEAVY_LOCKDOWN, shipManager.playerMatchID);
            return true;
        }
    }
}