using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

namespace Game.Ships.Agility.Client.CommandPerformers
{
    public class AgilityDodgeClientCommand : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData)
        {
            
            shipManager.shipLoadout.PrimaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments)
        {
           
            networker.Send();
            return true;
        }
    }
}