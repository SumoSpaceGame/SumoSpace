using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Common.Registry
{
    [Serializable]
    public struct PlayerID
    {
        /// <summary>
        /// Network ID
        /// </summary>
        [SerializeField] public int NetworkID;
        
        /// <summary>
        /// Used in future development 
        /// </summary>
        [SerializeField] public string ClientID;
        
        /// <summary>
        /// Match ID - ID that matches across all clients and server
        /// </summary>
        [SerializeField] public ushort MatchID;
        
        public override bool Equals(object obj)
        {

            if (obj.GetType() == typeof(uint))
            {
                return (uint) obj == NetworkID || (uint) obj == MatchID;
            }
            
            if (obj.GetType() != typeof(PlayerID))
            {
                return false;
            }

            return ((PlayerID) obj).MatchID == this.MatchID;
        }

        public override int GetHashCode()
        {
            return (int)NetworkID;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
        
        public static PlayerID Deserialize(string data)
        {
            return JsonUtility.FromJson<PlayerID>(data);
        }

        public override string ToString()
        {
            return $"(NetworkID, ClientID, MatchID) {NetworkID},{ClientID},{MatchID}";
        }
    }
}