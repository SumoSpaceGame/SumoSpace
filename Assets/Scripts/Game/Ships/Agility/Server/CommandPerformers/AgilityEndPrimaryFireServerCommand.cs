using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class AgilityEndPrimaryFireServerCommand: ICommand {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Stop(shipManager, true);
            networker.Send(packetData);
            return true;
        }
    }
}