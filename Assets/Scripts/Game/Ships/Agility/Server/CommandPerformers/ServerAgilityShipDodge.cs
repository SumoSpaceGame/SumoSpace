using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class ServerAgilityShipDodge: ICommand {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, true);
            networker.SendData(packetData, (int)CommandType.AGILITY_DODGE, shipManager.playerMatchID);
            return true;
        }
    }
}