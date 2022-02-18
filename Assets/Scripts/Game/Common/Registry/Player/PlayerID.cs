using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Common.Registry
{
    public struct PlayerID
    {
        /// <summary>
        /// Network ID
        /// </summary>
        [SerializeField] public uint ID;

        [SerializeField] public uint ClientID;
        public override bool Equals(object obj)
        {

            if (obj.GetType() == typeof(uint))
            {
                return (uint) obj == ClientID || (uint) obj == ID;
            }
            
            if (obj.GetType() != typeof(PlayerID))
            {
                return false;
            }

            return ((PlayerID) obj).ID == this.ID;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public static PlayerID Deserialize(string data)
        {
            return JsonUtility.FromJson<PlayerID>(data);
        }
    }
}