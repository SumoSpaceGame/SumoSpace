using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
    {

        public MasterSettings masterSettings;
        
        private void Awake()
        {
            MainPersistantInstances.Add(this);
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            
        }

        public override void CommandUpdate(RpcArgs args)
        {
            throw new NotImplementedException(); //Handle Command
        }

        public override void MovementUpdate(RpcArgs args)
        {
            if (networkObject.IsServer)
            {
                
                ServerMovementUpdate(args);
            }
            else
            {
                Debug.LogError("Client received movement update through input layer, this should not be the case");
                return;
            }
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<InputLayerNetworkManager>();
        }

        partial void ServerMovementUpdate(RpcArgs args);
    }
}
