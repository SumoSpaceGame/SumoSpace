using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName =  "PlayerIDRegistry", menuName = "Game Registry/Player ID Registry")]
    public class PlayerIDRegistry : ScriptableObject
    {
        private Dictionary<uint, PlayerID> playerIDs = new Dictionary<uint, PlayerID>();

        public bool RegisterPlayer(uint networkID, ushort matchID, string clientid = "")
        {
            Debug.Log("Registered client - " + networkID + " Match ID - " + matchID);

            if (playerIDs.ContainsKey(networkID))
            {
                return false;
            }
            
            playerIDs.Add(networkID, new PlayerID() {ID = networkID, MatchID = matchID, ClientID = clientid});
            
            return true;
        }
        
        
        
        public PlayerID Get(uint networkID)
        {
            //Debug.Log(clientID + 1);
            return playerIDs[networkID];
        }

        public PlayerID[] GetPlayers()
        {
            return playerIDs.Values.ToArray();
        }


        public bool TryGet(uint clientID, out PlayerID data)
        {
            return playerIDs.TryGetValue(clientID, out data);
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