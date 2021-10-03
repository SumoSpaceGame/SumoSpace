using System;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Agents;
using Game.Common.Instances;
using Game.Common.Phases;
using Random = UnityEngine.Random;

namespace Game.Common.Networking
{
    
    /// <summary>
    /// Manages all systems that facilitate the networks between player and server.
    /// This includes making sure all systems are running and set.
    /// </summary>
    public partial class GameNetworkManager : GameManagerBehavior, IGamePersistantInstance
    {

        
        private enum NetworkType {Client, Server}
        
        private NetworkType networkType = NetworkType.Client;
        
        public const int MATCH_PLAYER_SIZE = 2;
        
        
            
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });
            
            if (networkObject.IsServer)
            {
                networkType = NetworkType.Server;
                OnServerNetworkStart();
            }

            OnClientNetworkStart();
        }

        private void Update()
        {
            if (networkType == NetworkType.Server)
            {
                ServerUpdate();
            }
            else
            {
                ClientUpdate();
            }
            
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<GameNetworkManager>();
            Destroy(this.gameObject);
        }
    

        
        
        // Partial Methods
        
        /// <summary>
        /// Called when server network has started
        /// </summary>
        partial void OnServerNetworkStart();
        
        /// <summary>
        /// Called when client network has started
        /// </summary>
        partial void OnClientNetworkStart();
        
        
        /// <summary>
        /// Called to update server
        /// </summary>
        partial void ServerUpdate();
        
        /// <summary>
        /// Called to update client
        /// </summary>
        partial void ClientUpdate();

        
        
        
    }
}
