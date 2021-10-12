using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Common.Registry
{
    public struct PlayerID
    {
        [SerializeField] public uint ID;

        public override bool Equals(object obj)
        {
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