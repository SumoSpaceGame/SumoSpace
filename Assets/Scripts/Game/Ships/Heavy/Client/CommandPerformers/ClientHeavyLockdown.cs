using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Heavy.Client.CommandPerformers
{
    public class ClientHeavyLockdown : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            networker.SendData(CommandPacketData.Create(new byte[] { }), CommandType.HEAVY_LOCKDOWN, shipManager.playerMatchID);
            return true;
        }
    }
}