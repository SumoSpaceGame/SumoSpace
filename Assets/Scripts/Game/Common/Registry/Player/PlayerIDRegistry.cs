using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName =  "PlayerIDRegistry", menuName = "Game/Player ID Registry")]
    public class PlayerIDRegistry : ScriptableObject
    {
        private Dictionary<uint, PlayerID> playerIDs = new Dictionary<uint, PlayerID>();
        
        public PlayerID RegisterPlayer(uint clientID)
        {
            Debug.Log("Registered client - " + clientID);
            
            playerIDs[clientID] = new PlayerID() {ID = clientID};
            return playerIDs[clientID];
        }

        public PlayerID Get(uint clientID)
        {
            return playerIDs[clientID];
        }
    }
}