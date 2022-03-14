using System;
using UnityEngine;

namespace Game.Common.Settings
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameMatchSettings", menuName = "Game/Game Match Settings")]
    public class GameMatchSettings : ScriptableObject
    {
    
        public bool IsDataSynced { get; private set; }

        [SerializeField]public string SelectedMap = "TestMap";
    
        [SerializeField]public int TeamCount;
        [SerializeField]public int TeamSize;

        /// <summary>
        /// Match id of the player. On server side this is a temp variable that is not used.
        /// </summary>
        [SerializeField]public ushort ClientMatchID;
        [SerializeField] public int ClientTeam;
        [SerializeField] public int ClientTeamPosition;
        [SerializeField] public bool FriendlyFire;
    
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
