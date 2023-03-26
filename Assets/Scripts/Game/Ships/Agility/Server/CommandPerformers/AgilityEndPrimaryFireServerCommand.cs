using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class AgilityEndPrimaryFireServerCommand: ICommand {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Stop(shipManager, true);
            networker.SendData(packetData, CommandType.AGILITY_PRIMARY_FIRE_END, shipManager.playerMatchID);
            return true;
        }
    }
}