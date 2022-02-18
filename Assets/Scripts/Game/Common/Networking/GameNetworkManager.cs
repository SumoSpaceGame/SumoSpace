using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Phases;
using Game.Common.Settings;
using Random = UnityEngine.Random;

namespace Game.Common.Networking
{
    
    /// <summary>
    /// Manages all systems that facilitate the networks between player and server.
    /// This includes making sure all systems are running and set.
    /// </summary>
    public partial class GameNetworkManager : GameManagerBehavior, IGamePersistantInstance
    {

        public GameMatchSettings gameMatchSettings;
        public MasterSettings masterSettings;
        private enum NetworkType {Client, Server}
        
        private NetworkType networkType = NetworkType.Client;


        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        protected override void NetworkStart()
        {
            
            masterSettings.Reset();
            
            Physics2D.simulationMode = SimulationMode2D.Update;
            gameMatchSettings.Reset();
            
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });


            masterSettings.network.isServer = networkObject.IsServer;
            
            if (networkObject.IsServer)
            {
                networkType = NetworkType.Server;
                OnServerNetworkStart();
            }
            else
            {
                OnClientNetworkStart();
                networkObject.Networker.onPingPong += OnPing;
                networkObject.Networker.Ping();
                
            }
            
            StartCoroutine(StartDebugMessage());
            
            
            base.NetworkStart();
        }

        private double ping;
        
        private void OnPing(double i, NetWorker networker)
        {
            ping = i;
        }
        
        private IEnumerator StartDebugMessage()
        {
            while (networkObject.Networker.IsConnected)
            {
                yield return new WaitForSeconds(10 + (float)ping);
                networkObject.Networker.Ping();
                Debug.Log("Ping: " + ping + " Frame Time: " + Time.deltaTime);
            }
            
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


        public override void SyncMatchSettings(RpcArgs args)
        {

            if (networkObject.IsServer)
            {
                return;
            }
            
            gameMatchSettings.Sync(args.GetAt<string>(0));
            
        }
    }
}
