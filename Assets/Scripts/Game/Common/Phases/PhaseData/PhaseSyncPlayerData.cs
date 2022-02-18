using System.Text;
using UnityEngine;

namespace Game.Common.Phases.PhaseData
{
    public class PhaseSyncPlayerData
    {

        public struct Data
        {
            [SerializeField]
            public uint TeamID;
            [SerializeField]
            public uint PlayerID;
            [SerializeField]
            public uint ClientID;
        }


        public static byte[] Serialized(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static string Deserialized(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}