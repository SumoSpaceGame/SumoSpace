using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    /// <summary>
    /// Player data is a set and forget data type. You should not update player data. Any player initialization data will get stored here.
    ///
    /// If wanting to change stats or anything of the sort, you should create a secondary registry for that.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStaticDataRegistry", menuName = REGISTRY_MENU_NAME + "Player Static Data Registry")]
    public class PlayerStaticDataRegistry : RegistryScriptableObject<PlayerID, PlayerStaticData>
    {
        public override bool Add(PlayerID key, PlayerStaticData value)
        {
            value.GlobalID = key;
            return base.Add(key, value);
        }

    }

    /// <summary>
    /// Is a class to keep reference
    /// </summary>
    [Serializable]
    public class PlayerStaticData
    {
        /// <summary>
        /// Main ID of the player. This is global game, so even out of this match, it will be unique
        /// </summary>
        [NonSerialized]public PlayerID GlobalID;

        [SerializeField]
        public int TeamID;
        [SerializeField]
        public int TeamPosition;
        [SerializeField]
        public string PlayerName;
    }
    
    
}