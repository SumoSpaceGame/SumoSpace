using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Client.CommandPerformers
{
    public class HeavyBurstClientCommand : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            networker.SendData(CommandPacketData.Create(new byte[] { }), CommandType.HEAVY_BURST, shipManager.playerMatchID);
            return true;
        }
    }
}