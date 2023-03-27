using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Client.CommandPerformers
{
    public class HeavyEndPrimaryFireClientCommand: ICommandPerformer {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Stop(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments) {
            networker.Send();
            return true;
        }
    }
}