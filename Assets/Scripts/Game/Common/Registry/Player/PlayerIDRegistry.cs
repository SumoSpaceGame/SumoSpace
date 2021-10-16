using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName =  "PlayerIDRegistry", menuName = "Game/Player ID Registry")]
    public class PlayerIDRegistry : ScriptableObject
    {
        private Dictionary<uint, PlayerID> playerIDs = new Dictionary<uint, PlayerID>();

        public bool RegisterPlayer(uint clientID)
        {
            Debug.Log("Registered client - " + clientID);

            if (playerIDs.ContainsKey(clientID))
            {
                return false;
            }
            
            playerIDs.Add(clientID, new PlayerID() {ID = clientID});
            
            return true;
        }

        public PlayerID Get(uint clientID)
        {
            return playerIDs[clientID];
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