using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using Game.Common.Networking.Commands;
using Game.Common.Settings;

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
            
            if (InstanceFinder.IsServer)
            {
                ServerStart();
            }
            else
            {
                ClientStart();
            }
            
            
        }
        


        [ServerRpc(RequireOwnership = false)]
        public void ServerCommandUpdate(CommandType commandID, byte[] data, ushort matchID, NetworkConnection conn = null)
        {
            _commandHandlerNetworkManager.HandleRPC(commandID, data, matchID, conn);
            
        }
        
        [ObserversRpc]
        public void ClientCommandUpdate(CommandType commandID, byte[] data, ushort matchID)
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
