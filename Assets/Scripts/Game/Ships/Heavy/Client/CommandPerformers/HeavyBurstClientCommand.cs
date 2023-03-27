using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Client.CommandPerformers
{
    public class HeavyBurstClientCommand : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments)
        {
            networker.Send();
            return true;
        }
    }
}