using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using Game.Common.Networking.Commands;
using Game.Common.Settings;
using Game.Ships.Agility.Client.CommandPerformers;
using Game.Ships.Agility.Server.CommandPerformers;
using Game.Ships.Heavy.Client.CommandPerformers;
using Game.Ships.Heavy.Server.CommandPerformers;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        
        public MasterSettings masterSettings;

        private CommandHandlerNetworkManager _commandHandlerNetworkManager;
        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            MainPersistantInstances.TryAdd(this);
            
            _commandHandlerNetworkManager = new CommandHandlerNetworkManager(this, masterSettings);

            var commands = CommandTypes.GetList();
            RegisterAllCommands(commands);

        }


        public void RegisterAllCommands( CommandTypes.CommandInfo[] commands)
        {
            foreach (var command in commands)
            {
                this.RegisterCommand(command);
            }
        }

        public void RegisterCommand( CommandTypes.CommandInfo command)
        {
            
            if (InstanceFinder.IsServer)
            {
                List<KeyValuePair<string, ICommand>> receivers = new List<KeyValuePair<string, ICommand>>();
                
                receivers.Add((new KeyValuePair<string, ICommand>(command.key, command.receiver)));
                
                _commandHandlerNetworkManager.InitializeServerCommands(receivers);
                
            }
            else
            {
                List<KeyValuePair<string, ICommandPerformer>> performers = new List<KeyValuePair<string, ICommandPerformer>>();
                
                performers.Add((new KeyValuePair<string, ICommandPerformer>(command.key, command.performer)));
               
                _commandHandlerNetworkManager.InitializeClientCommands(performers);
            }
        }



        [ServerRpc(RequireOwnership = false)]
        public void ServerCommandUpdate(string commandID, byte[] data, ushort matchID, NetworkConnection conn = null)
        {
            _commandHandlerNetworkManager.HandleRPC(commandID, data, matchID, conn);
            
        }
        
        [ObserversRpc]
        public void ClientCommandUpdate(string commandID, byte[] data, ushort matchID)
        {
            _commandHandlerNetworkManager.HandleRPC(commandID, data, matchID);
        }


        private void OnDestroy()
        {
            MainPersistantInstances.Remove<InputLayerNetworkManager>();
        }
        
        


        partial void ServerStart();
        partial void ClientStart();
    }
}
