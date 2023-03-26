using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Client.CommandPerformers
{
    public class AgilityBeginPrimaryFireClientCommand: ICommandPerformer {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData) {
            shipManager.shipLoadout.PrimaryFire.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
            shipManager.shipLoadout.PrimaryFire.QuickExecute(shipManager, false);
            networker.SendData(CommandPacketData.Create(new byte[]{}), CommandType.AGILITY_PRIMARY_FIRE_START, shipManager.playerMatchID);
            return true;
        }
    }
}