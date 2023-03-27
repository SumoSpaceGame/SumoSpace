using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Client.CommandPerformers
{
    public class ClientAgilityBeginPrimaryFire: ICommandPerformer {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments) {
            shipManager.shipLoadout.PrimaryFire.QuickExecute(shipManager, false);
            networker.Send();
            return true;
        }
    }
}