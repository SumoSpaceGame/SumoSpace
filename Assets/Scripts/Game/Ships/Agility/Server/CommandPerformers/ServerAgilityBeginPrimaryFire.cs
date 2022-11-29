using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class ServerAgilityBeginPrimaryFire: ICommand {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Execute(shipManager, true);
            networker.SendData(packetData, CommandType.AGILITY_PRIMARY_FIRE_START, shipManager.playerMatchID);
            return true;
        }
    }
}