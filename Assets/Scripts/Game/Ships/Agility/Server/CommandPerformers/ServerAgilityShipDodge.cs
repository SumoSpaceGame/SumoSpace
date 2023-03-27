using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class ServerAgilityShipDodge: ICommand {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, true);
            networker.Send(packetData);
            return true;
        }
    }
}