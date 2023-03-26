using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Server.CommandPerformers
{
    public class HeavyBurstServerCommand : ICommand
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, true);
            networker.SendData(packetData, CommandType.HEAVY_BURST, shipManager.playerMatchID);
            return true;
        }
    }
}