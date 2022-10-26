using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace UnityTemplateProjects.Game.Client.Gameplay.Abilities.CommandPerformers
{
    public class ClientAgilityDodge : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            networker.SendData(CommandPacketData.Create(new byte[] { }), CommandType.AGILITY_DODGE,
                shipManager.playerMatchID);
            return true;
        }
    }
}