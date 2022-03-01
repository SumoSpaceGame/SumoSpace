using System;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEngine;
using UnityEngine.Serialization;

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
        public void Reset()
        {
            if(playerShips != null) playerShips.Reset();
            playerStaticDataRegistry.Reset();
            playerGameDataRegistry.Reset();
            playerIDRegistry.Reset();
            
        }

        public ShipManager GetShip(uint networkID)
        {
            if (playerStaticDataRegistry.TryGet(playerIDRegistry.Get(networkID), out var data))
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
        
        public void CleanupPlayer(uint networkID)
        {
            Debug.LogWarning("Cleaning player from game, should be done on reset, or ready up");

            var playerID = playerIDRegistry.Get(networkID);
            
            playerGameDataRegistry.Remove(playerID);
            playerStaticDataRegistry.Remove(playerID);
            
            playerIDRegistry.RemovePlayerID(networkID);

        }

        public void RegisterPlayer(uint networkID, ushort matchID, string clientID)
        {
            playerIDRegistry.RegisterPlayer(networkID, matchID, clientID);
            var savedID = playerIDRegistry.Get(networkID);


            playerStaticDataRegistry.Add(savedID, new PlayerStaticData() { });
            playerGameDataRegistry.Add(savedID, new PlayerGameData() { });
        }
    }
}
