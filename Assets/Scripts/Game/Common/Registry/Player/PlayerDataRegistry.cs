using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName = "PlayerDataRegistry", menuName = "Game/Player Data Registry")]
    public class PlayerDataRegistry : ScriptableObject
    {
        private Dictionary<PlayerID, PlayerData> playerData = new Dictionary<PlayerID, PlayerData>();

        
        public void Add(PlayerID id, PlayerData data)
        {
            data.GlobalID = id;
            playerData.Add(id, data);
        }

        public bool Get(PlayerID id, out PlayerData data)
        {
            return playerData.TryGetValue(id, out data);
        }


    }

    public struct PlayerData
    {
        /// <summary>
        /// Main ID of the player. This is global game, so even out of this match, it will be unique
        /// </summary>
        public PlayerID GlobalID;
        public ushort PlayerMatchID;
    }
    
    
}