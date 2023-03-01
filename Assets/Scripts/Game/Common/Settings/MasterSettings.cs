using System;
using FishNet;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Map;
using Game.Common.Registry;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Settings
{
    [CreateAssetMenu(fileName = "Settings" ,menuName = "Game Registry/Master Settings", order = 0)]
    public class MasterSettings : ScriptableObject
    {
        #region Debug Region
        
        // Private setter, only should be set through the scriptable object or a script
        [SerializeField]
        private bool _debug;

        /// <summary>
        /// Used to enable debug features.
        /// It could be about error check or such
        /// </summary>
        public bool IsDebug
        {
            get { return _debug; }
        }
        
        /// <summary>
        /// Should only be used for build items
        /// </summary>
        /// <param name="debug"></param>
        public void SetDebug(bool debug)
        {
            _debug = debug;
        }
        
        #endregion
        
        [Header("Main settings")]
        public GameMatchSettings matchSettings;
        
        public NetworkSettings network;

        /// <summary>
        /// Used to check if this is a server being initiated
        /// </summary>
        public bool InitServer = false;

        [FormerlySerializedAs("ClientAutoConnect")] public bool ClientDebugAutoConnect = false;

        [Space(4)] 
        [Header("Network config")] 
        public string ServerAddress = "localhost";
        public ushort ServerPort;

        #region Registries Region

        [Space(4)]
        [Header("Registries")]
        public PlayerShips playerShips;
        
        public PlayerStaticDataRegistry playerStaticDataRegistry;
        
        public PlayerIDRegistry playerIDRegistry;

        public PlayerGameDataRegistry playerGameDataRegistry;
        
        public MapRegistry mapRegistry;

        #endregion
        
        [Space(4)]
        [Header("Misc")]
        public ShipPrefabList ShipPrefabList;

        public void ResetMatchData()
        {
            if(playerShips != null) playerShips.Reset();
            playerStaticDataRegistry.Reset();
            playerGameDataRegistry.Reset();
            playerIDRegistry.Reset();
            
        }

        public ShipManager GetShipByMatchID(ushort matchID)
        {
            if (playerStaticDataRegistry.TryGet(playerIDRegistry.GetByMatchID(matchID), out var data))
            {
                if (playerShips.TryGet(data.GlobalID, out ShipManager manager))
                {
                    return manager;
                }
            } 
            
            return null;
        }

        public PlayerID[] GetPlayerIDs()
        {
            return playerIDRegistry.GetPlayers();
        }
        
        public int GetPlayerCount()
        {
            return playerIDRegistry.GetPlayers().Length;
        }

        
        //Ease of use commands, helper for other classes, should be moved later
        // TODO: Organize the locations of these methods
        
        public void CleanupPlayer(int networkID)
        {
            Debug.LogWarning("Cleaning player from game, should be done on reset, or ready up");
            
            
            PlayerID playerID;

            if (!playerIDRegistry.TryGetByNetworkID(networkID, out playerID))
            {
                // Nothing to clean up, they are not registered
                return;
            }
                
                
            playerGameDataRegistry.Remove(playerID);
            playerStaticDataRegistry.Remove(playerID);
            
            playerIDRegistry.RemovePlayerID(playerID.MatchID);

        }

        public PlayerID RegisterPlayer(int networkID, ushort matchID, string clientID,
            PlayerStaticData staticData = null)
        {

            if (playerIDRegistry.HasClientID(clientID) || playerIDRegistry.HasMatchID(matchID) ||
                playerIDRegistry.HasNetworkID(networkID))
            {
                Debug.LogError("Player id already contains one of these values. Failed to register " + clientID);
                return new PlayerID();
            }
                
            playerIDRegistry.RegisterPlayer(networkID, matchID, clientID);
            var savedID = playerIDRegistry.GetByMatchID(matchID);

            if (staticData == null)
            {
                playerStaticDataRegistry.Add(savedID, new PlayerStaticData() { });
            }
            else
            {
                playerStaticDataRegistry.Add(savedID, staticData);
            }
            playerGameDataRegistry.Add(savedID, new PlayerGameData() { });

            return savedID;
        }

        public PlayerStaticData GetPlayerData(PlayerID id)
        {
            return playerStaticDataRegistry.Get(id);
        }
        
        public PlayerStaticData GetPlayerData(ushort matchID)
        {
            if (playerIDRegistry.TryGetByMatchID(matchID, out var id))
            {
                return GetPlayerData(id);
            }
            else
            {
                return null;
            }
        }

        public ShipManager GetPlayerShip()
        {
            if (InstanceFinder.IsServer)
            {
                Debug.LogError("Tried to get the player ship of a server. Should not happen");
                return null;
            }

            var data = playerIDRegistry.GetByMatchID(matchSettings.ClientMatchID);

            if (!playerShips.Has(data)) return null;

            return playerShips.Get(data);
        }


        public void DebugLogPlayerStatic()
        {
            var playerIDs = GetPlayerIDs();

            foreach (var id in playerIDs)
            {
                var staticData = playerStaticDataRegistry.Get(id);
                Debug.Log(id.MatchID + " TeamID: " + staticData.TeamID + " TeamPosition: " + staticData.TeamPosition + " PlayerName: " + staticData.PlayerName);
            }
        }
    }
}
