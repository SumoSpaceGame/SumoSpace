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
        
        /// <summary>
        /// Whenever the client wants to activate a command, it sends it to the server
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void CommandUpdate(RpcArgs args)
        {
            throw new NotImplementedException(); // TODO: Handle Command
        }

       
        /// <summary>
        /// Server receives movement updates, so it can update the ships
        /// </summary>
        /// <param name="args"></param>
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
