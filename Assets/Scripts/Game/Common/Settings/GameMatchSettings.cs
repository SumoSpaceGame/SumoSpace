using System;
using Game.Common.Map;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Settings
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameMatchSettings", menuName = "Game/Game Match Settings")]
    public class GameMatchSettings : ScriptableObject
    {
    
        public bool IsDataSynced { get; private set; }

        /// <summary>
        /// Time in minutes
        /// </summary>
        [SerializeField] public int TeamCount;
        [SerializeField] public int TeamSize;
        
        [NonSerialized] public MapRegistry.MapItem SelectedMapItem;
        /// <summary>
        /// Match id of the player. On server side this is a temp variable that is not used.
        /// </summary>
        [SerializeField] public ushort ClientMatchID;
        [SerializeField] public int ClientTeam;
        [SerializeField] public int ClientTeamPosition;
        [SerializeField] public bool ClientIsSpectator;
        
        /// <summary>
        /// Match flag settings
        /// </summary>
        [SerializeField] public bool FriendlyFire;
        [SerializeField] public bool MatchStarted = false;
        // Restarts/stops the server when a player has left
        [SerializeField] public bool ServerRestartOnLeave = false;
        [SerializeField] public bool AllowSpectators = false;
        
        /// <summary>
        /// Match data, who is a player, and their team information
        ///
        /// If they are not an active player (spectator and such), there will be no info here
        /// For delayed spectators (3mins or so)
        /// </summary>
        [Serializable]
        public struct PlayerMatchData
        {
            [SerializeField] public PlayerID playerID;
            [SerializeField] public int clientTeam;
            [SerializeField] public int clientTeamPosition;

        }
        
        public int MaxPlayerCount
        {
            get
            {
                return TeamSize * TeamCount;
            }
        }


        private void Awake()
        {
            IsDataSynced = false;
        }

        public void Reset()
        {
            IsDataSynced = false;
        }

        public void Sync(string data)
        {
            Debug.Log("Syncing Game Match Settings - " + data);
        
            JsonUtility.FromJsonOverwrite(data, this);

            IsDataSynced = true;
        }

    
        public string GetSerialized()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
