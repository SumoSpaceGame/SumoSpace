using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Phases;
using Game.Common.Settings;
using Game.Common.Util;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
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

        public string clientID;
        
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        protected override void NetworkStart()
        {
            // TODO: Create a better system to make sure the UUID is unique
            // This can be done by having a quick server client handshake, but
            // the client should already have and ID that links them to the account.
            // In the future the id can be an handshake id when the client is given a temporary code.
            // Then the server takes the temporary code and checks it against the account server.
            // If the account server finds the temporary code (its not expired and such) then 
            // the sever should accept it.
            // The server should accept the temporary code + the true account ID
            // This can be highjacked, but it is highly unlikely
            clientID = SystemInfo.deviceUniqueIdentifier + "P" + Process.GetCurrentProcess().Id + "R" + Random.Range(int.MinValue, int.MaxValue);
            
            masterSettings.Reset();
            
            Physics2D.simulationMode = SimulationMode2D.Update;
            gameMatchSettings.Reset();
            
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });
            

            masterSettings.network.isServer = networkObject.IsServer;
            
            if (networkObject.IsServer)
            {
                clientID = "";
                networkType = NetworkType.Server;
                OnServerNetworkStart();
                networkObject.Networker.disconnected += OnServerNetworkClose;
                networkObject.Networker.playerDisconnected += OnServerNetworkPlayerDisconnected;
            }
            else
            {
                OnClientNetworkStart();
                networkObject.Networker.disconnected +=  OnClientNetworkClose;
                
            }

            this.networkObject.Networker.disconnected += (sender) =>
            {
                MainThreadManager.Run(() =>
                {
                    StopMatch();
                });
            };
            
            
            
            base.NetworkStart();
        }


        private void Update()
        {
            if (networkType == NetworkType.Server)
            {
                //Debug.Log(1.0f/Time.deltaTime);
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


        /// <summary>
        /// Destroys and removes all match instance items
        /// </summary>
        public void StopMatch()
        {
            
            if(NetworkManager.Instance != null) NetworkManager.Instance.Disconnect();

            masterSettings.Reset();
            
            PersistantUtility.DestroyNetworkPersistant(Destroy);
            
            SceneManager.LoadScene(1);
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
        /// Called when server network has closed
        /// </summary>
        partial void OnServerNetworkClose(NetWorker sender);
        
        /// <summary>
        /// Called when client network has closed
        /// </summary>
        partial void OnClientNetworkClose(NetWorker sender);
        
        /// <summary>
        /// Called when player left
        /// </summary>
        partial void OnServerNetworkPlayerDisconnected(NetworkingPlayer player, NetWorker sender);
        
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

        public override void RequestClientID(RpcArgs args)
        {
            if (networkObject.IsServer)
            {
                if (args.Args.Length != 1)
                {
                    Debug.LogError("Failed to process request client ID, client did not send their id to the server.");
                    return;
                }
                
                ServerRecieveClientID(args.Info.SendingPlayer, args.GetNext<string>());
                return;
            }
            
            // When the third party server auth gets created, any auth special temp code should only get sent to the server
            // Checking if the receiving player is from the server
            networkObject.SendRpc(args.Info.SendingPlayer, RPC_REQUEST_CLIENT_I_D, this.clientID);
        }


        partial void ServerRecieveClientID(NetworkingPlayer player, string id);
        
        
        
        public override void RequestServerJoin(RpcArgs args)
        {
            if (networkObject.IsServer)
            {
                ServerRecieveClientJoinRequest(args.Info.SendingPlayer, args.GetNext<string>());
            }
            else
            {
                Debug.LogError("Tried to request serve join on client? From " + args.Info.SendingPlayer.Ip);
                return;
            }
        }

        partial void ServerRecieveClientJoinRequest(NetworkingPlayer player, string requestingClientID);
        
        

        public override void UpdatePlayerNetworkID(RpcArgs args)
        {
            if (networkObject.IsServer) return;
            
            ClientUpdatePlayerNetworkID(args.GetNext<uint>(), args.GetNext<ushort>());
        }

        partial void ClientUpdatePlayerNetworkID(uint networkID, ushort matchID);
    }
}
