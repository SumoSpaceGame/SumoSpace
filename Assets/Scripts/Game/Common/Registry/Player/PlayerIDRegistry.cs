using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName =  "PlayerIDRegistry", menuName = "Game Registry/Player ID Registry")]
    public class PlayerIDRegistry : ScriptableObject
    {
        private Dictionary<ushort, PlayerID> playerIDs = new Dictionary<ushort, PlayerID>();

        public bool RegisterPlayer(int networkID, ushort matchID, string clientID = "")
        {
            if (HasClientID(clientID) || HasMatchID(matchID) ||
                HasNetworkID(networkID))
            {
                Debug.Log("Failed to register client - " + networkID + " Match ID - " + matchID + " clientID - "  + clientID);

                return false;
            }
            Debug.Log("Registered client - " + networkID + " Match ID - " + matchID + " clientID - " + clientID);

            
            playerIDs.Add(matchID, new PlayerID() {NetworkID = networkID, MatchID = matchID, ClientID = clientID});
            
            return true;
        }

        public bool RemovePlayerID(ushort matchID)
        {
            return playerIDs.Remove(matchID);
        }


        public PlayerID GetByMatchID(ushort matchID)
        {
            try
            {
                return playerIDs[matchID];
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get player by match ID  {matchID}");
                Console.WriteLine(e);
                throw;
            }
        }
        
        public PlayerID GetByNetworkID(uint networkID)
        {
            //Debug.Log(clientID + 1);
            try
            {
                foreach (var playerID in playerIDs.Values)
                {
                    if (playerID.NetworkID == networkID)
                    {
                        return playerID;
                    }
                }

                throw new Exception($"Failed to get player id by network ID {networkID}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get player by network ID  {networkID}");
                Console.WriteLine(e);
                throw;
            }
        }
        public PlayerID GetByClientID(string clientID)
        {
            //Debug.Log(clientID + 1);
            try
            {
                foreach (var playerID in playerIDs.Values)
                {
                    if (playerID.ClientID.Equals(clientID))
                    {
                        return playerID;
                    }
                }

                throw new Exception($"Failed to get player id by network ID {clientID}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get player by network ID  {clientID}");
                Console.WriteLine(e);
                throw;
            }
        }

        public bool HasNetworkID(int networkID)
        {
            foreach (var playerID in playerIDs.Values)
            {
                if (playerID.NetworkID.Equals(networkID))
                {
                    return true;
                }
            }

            return false;
        }
        public bool HasMatchID(ushort matchID)
        {
            return playerIDs.ContainsKey(matchID);
        }
        public bool HasClientID(string clientID)
        {
            foreach (var playerID in playerIDs.Values)
            {
                if (playerID.ClientID.Equals(clientID))
                {
                    return true;
                }
            }

            return false;
        }

        public PlayerID[] GetPlayers()
        {
            return playerIDs.Values.ToArray();
        }

        public bool TryGetByMatchID(ushort matchID, out PlayerID data)
        {
            return playerIDs.TryGetValue(matchID, out data);
        }

        public bool TryGetByNetworkID(int networkID, out PlayerID data)
        {
            foreach (var playerID in playerIDs.Values)
            {
                if (playerID.NetworkID == networkID)
                {
                    data = playerID;
                    return true;
                }
            }

            data = new PlayerID();
            return false;
        }

        public void UpdatePlayerIDNetworkID(ushort matchID, int networkID)
        {
            if (!playerIDs.ContainsKey(matchID))
            {
                Debug.LogError("Failed to update a player ID " + matchID + " " + networkID);
                return;
            }

            var updatedID = playerIDs[matchID];
            updatedID.NetworkID = networkID;
            playerIDs[matchID] = updatedID;
        }
        
        /// <summary>
        /// Resets the stored data for PlayerIDs. This should never be called unless you are cleaning
        /// </summary>
        public void Reset()
        {
            Debug.Log("Clearing the PlayerID database");
            playerIDs.Clear();
        }
    }
}