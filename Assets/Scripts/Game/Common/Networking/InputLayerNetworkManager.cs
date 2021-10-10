using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Instances;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
    {
        private void Awake()
        {
            MainPersistantInstances.Add(this);
            DontDestroyOnLoad(this);
        }

        public override void CommandUpdate(RpcArgs args)
        {
            throw new NotImplementedException(); //Handle Command
        }

        public override void MovementUpdate(RpcArgs args)
        {  
        
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<InputLayerNetworkManager>();
        }
    }
}
