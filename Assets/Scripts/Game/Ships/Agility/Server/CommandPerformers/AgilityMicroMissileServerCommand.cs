using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Server.CommandPerformers
{
    public class AgilityMicroMissileServerCommand: ICommand {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, true);
            networker.SendData(packetData, CommandType.AGILITY_MICRO_MISSLES, shipManager.playerMatchID);
            return true;
        }
    }
}
