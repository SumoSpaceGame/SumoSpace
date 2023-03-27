using Game.Common.Gameplay.Ship;

namespace Game.Common.Gameplay.Commands.GenericCommands
{
    public class ClientGenericActivateCommand : ICommandPerformer
    {
        private Settings settings;

        public struct Settings
        {
            public bool DoQuickExecuteOnPerform;
        }
        public ClientGenericActivateCommand() {}

        public ClientGenericActivateCommand(Settings settings)
        {
            this.settings = settings;
        }
        
        
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            shipManager.shipLoadout.ActivateCommand(shipManager, false, networker.commandType);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments) {
            if (settings.DoQuickExecuteOnPerform)
            {
                shipManager.shipLoadout.ActivateCommand(shipManager, false, networker.commandType, true);
            }
            
            networker.Send();
            return true;
        }
    }
}