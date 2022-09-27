using System;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Settings
{
    [CreateAssetMenu(fileName = "Settings" ,menuName = "Game Registry/Master Settings", order = 0)]
    public class MasterSettings : ScriptableObject
    {
        [Header("Main settings")]
        public GameMatchSettings matchSettings;
        
        public NetworkSettings network;

        /// <summary>
        /// Used to check if this is a server being initiated
        /// </summary>
        public bool InitServer = false;

        [Space(4)] 
        [Header("Network config")] 
        public string ServerAddress = "localhost";
        public ushort ServerPort;
        
        [Space(4)]
        [Header("Registries")]
        public PlayerShips playerShips;
        
        public PlayerStaticDataRegistry playerStaticDataRegistry;
        
        public PlayerIDRegistry playerIDRegistry;

        public PlayerGameDataRegistry playerGameDataRegistry;
        
        [Space(4)]
        [Header("Misc")]
        public ShipPrefabList ShipPrefabList;

        /// <summary>
        /// Called on start, will process args and set required data
        /// </summary>
        private void Awake()
        {
            string[] args = System.Environment.GetCommandLineArgs ();
            string input = "";
            for (int i = 0; i < args.Length; i++) {
                if (i + 1 >= args.Length) continue;
                
                switch (args[i])
                {
                    case "-teamsize":
                        matchSettings.TeamSize = Int32.Parse(args[i + 1]);
                        break;
                    case "-teamamount":
                        matchSettings.TeamCount = Int32.Parse(args[i + 1]);
                        break;
                    case "-updateinterval":
                        // TODO: Add update interval
                        //network.updateInterval = UInt64.Parse(args[i + 1]);
                        break;
                    
                }
                
                
            }
        }

        public void Reset()
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
    }
}
