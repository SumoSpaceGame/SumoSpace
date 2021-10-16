using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    /// <summary>
    /// Player data is a set and forget data type. You should not update player data. Any player initialization data will get stored here.
    ///
    /// If wanting to change stats or anything of the sort, you should create a secondary registry for that.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerDataRegistry", menuName = "Game/Player Data Registry")]
    public class PlayerDataRegistry : ScriptableObject
    {
        private Dictionary<PlayerID, PlayerData> playerData = new Dictionary<PlayerID, PlayerData>();

        
        public bool Add(PlayerID id, PlayerData data)
        {
            if (playerData.ContainsKey(id))
            {
                return false;
            }
            
            data.GlobalID = id;
            playerData.Add(id, data);
            return true;
        }

        public bool TryGet(PlayerID id, out PlayerData data)
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