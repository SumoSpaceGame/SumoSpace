using System.Collections.Generic;
using System.Linq;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Networking.Utility
{
    public class PlayerCounter
    {
        private HashSet<PlayerID> players = new HashSet<PlayerID>();
        private int maxPlayerCount = 0;
        
        public PlayerCounter(int maxPlayer)
        {
            maxPlayerCount = maxPlayer;
        }
        
        public void Register(PlayerID playerID)
        {
            if (players.Contains(playerID))
            {
                Debug.LogError("Tried to count and already counted player player");
                return;
            }

            if (players.Count + 1 > maxPlayerCount)
            {
                Debug.LogError("Tried to count a player above max count, this should not happen");
                return;
            }
            
            players.Add(playerID);
        }

        public bool IsFull()
        {
            return players.Count == maxPlayerCount;
        }

        public PlayerID[] GetPlayers()
        {
            return players.ToArray();
        }
    }
}