using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class AgilityShipDodgeServerCommand: ICommand {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, true);
            networker.SendData(packetData, CommandType.AGILITY_DODGE, shipManager.playerMatchID);
            return true;
        }
    }
}