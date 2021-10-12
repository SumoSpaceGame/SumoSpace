using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName = "PlayerDataRegistry", menuName = "Game SO/Player Data Registry")]
    public class PlayerDataRegistry : ScriptableObject
    {
        private Dictionary<PlayerID, PlayerData> playerData = new Dictionary<PlayerID, PlayerData>();

        public PlayerIDRegistry
        
        public void Add(PlayerID id, PlayerData data)
        {
            playerData.Add(id, data);
        }

        public bool Get(PlayerID id, out PlayerData data)
        {
            return playerData.TryGetValue(id, out data);
        }

        public bool Get(uint networkID, out PlayerData data)
        {
            
        }


    }

    public struct PlayerData
    {
        /// <summary>
        /// Main ID of the player. This is global game, so even out of this match, it will be unique
        /// </summary>
        public uint PlayerID; // TODO: Replace with a real identifier
        public ushort PlayerMatchID;
    }
    
    
}