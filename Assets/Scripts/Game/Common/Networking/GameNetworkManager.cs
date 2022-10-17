using System;
using System.Diagnostics;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Game.Common.Instances;
using Game.Common.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Common.Networking
{
    
    /// <summary>
    /// Manages all systems that facilitate the networks between player and server.
    /// This includes making sure all systems are running and set.
    /// </summary>
    public partial class GameNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {

        public GameMatchSettings gameMatchSettings;
        public MasterSettings masterSettings;
        private enum NetworkType {Client, Server}
        
        private NetworkType networkType = NetworkType.Client;

        public string clientID;

        private void Awake()
        {
            NetworkObject.SetIsGlobal(true);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
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
            
            gameMatchSettings.Reset();
            
            MainPersistantInstances.TryAdd(this);
            
            var isServer = InstanceFinder.IsServer;
            masterSettings.network.isServer = isServer;
            
            if (IsServer)
            {
                clientID = "";
                networkType = NetworkType.Server;
                OnServerNetworkStart();
                InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoveConnectionState;
                InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;
            }
            else
            {
                OnClientNetworkStart();
                InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
            }

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
            if (InstanceFinder.NetworkManager.Initialized)
            {
                if (InstanceFinder.IsServer)
                {
                    InstanceFinder.ServerManager.StopConnection(true);
                }
                else
                {
                    InstanceFinder.ClientManager.StopConnection();
                }
            }

            masterSettings.Reset();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
        }

        private void OnRemoveConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case RemoteConnectionState.Stopped:
                    OnServerNetworkPlayerDisconnected(conn);
                    ServerOnPlayerDisconnected(conn);
                    break;
                case RemoteConnectionState.Started:
                    ServerOnPlayerConnected(conn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Stopped:
                    OnClientNetworkClose();
                    
                    StopMatch();
                    break;
                case LocalConnectionState.Starting:
                    break;
                case LocalConnectionState.Started:
                    break;
                case LocalConnectionState.Stopping:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Stopped:
                    OnServerNetworkClose();
                    
                    StopMatch();
                    break;
                case LocalConnectionState.Starting:
                    break;
                case LocalConnectionState.Started:
                    break;
                case LocalConnectionState.Stopping:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        partial void OnServerNetworkClose();
        
        /// <summary>
        /// Called when client network has closed
        /// </summary>
        partial void OnClientNetworkClose();
        
        /// <summary>
        /// Called when player left
        /// </summary>
        partial void OnServerNetworkPlayerDisconnected(NetworkConnection player);
        
        /// <summary>
        /// Called to update server
        /// </summary>
        partial void ServerUpdate();
        
        /// <summary>
        /// Called to update client
        /// </summary>
        partial void ClientUpdate();

        [TargetRpc]
        public void SyncMatchSettings(NetworkConnection conn, string data)
        {
            gameMatchSettings.Sync(data);
        }
        
        
        [ServerRpc(RequireOwnership = false)]
        public void RequestServerJoin(string clientID, NetworkConnection player = null)
        {
            ServerRecieveClientJoinRequest(player, clientID);
        }

        partial void ServerRecieveClientJoinRequest(NetworkConnection player, string requestingClientID);
        
        

        [ObserversRpc]
        public void UpdatePlayerNetworkID(int playerNetworkID, ushort matchID)
        {
            ClientUpdatePlayerNetworkID(playerNetworkID, matchID);
        }

        partial void ClientUpdatePlayerNetworkID(int networkID, ushort matchID);

        private void OnApplicationQuit()
        {
            if (IsServer)
            {
                InstanceFinder.ServerManager.StopConnection(true);
            }
        }
    }
}
