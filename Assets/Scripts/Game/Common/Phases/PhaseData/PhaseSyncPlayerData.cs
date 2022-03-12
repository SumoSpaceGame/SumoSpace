using System;
using System.Text;
using BeardedManStudios.SimpleJSON;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Phases.PhaseData
{
    
    public class PhaseSyncPlayerData
    {
        public const int TrueFlag = 1 << 1;
        
        [Serializable]
        public struct Data
        {
            [SerializeField] public bool valid; 
            [SerializeField] public PlayerID[] playerIDs;
            [SerializeField] public ulong serverUpdateInterval;
            [SerializeField] public bool friendlyFire;
        }
        
        

        public static byte[] Serialized(Data data)
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            byte[] joinedData = new byte[jsonData.Length + 1];
            jsonData.CopyTo(joinedData, 1);
            joinedData[0] = TrueFlag;
            return joinedData;
        }

        public static Data Deserialize(byte[] data)
        {
            if (data[0] != TrueFlag)
            {
                return new Data() {valid = false};
            }

            byte[] jsonData = new byte[data.Length - 1];
            Array.Copy(data, 1, jsonData, 0, jsonData.Length);
            
            var finishedData = JsonUtility.FromJson<Data>(Encoding.UTF8.GetString(jsonData));
            finishedData.valid = true;
            return finishedData;
        }
    }
}