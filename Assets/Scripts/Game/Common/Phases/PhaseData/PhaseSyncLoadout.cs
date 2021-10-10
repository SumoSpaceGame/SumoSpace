using System;
using UnityEngine;

namespace Game.Common.Phases.PhaseData
{
    public class PhaseSyncLoadout
    {

        public struct SyncData
        {
            public int PlayerCount;
            public int[] PlayerIDs;
            public int[] PlayerSelections;
        }
        
        /// <summary>
        /// Decodes byte[] to data used for sync. Byte structure consists of
        /// 0 = playerCount
        /// nextSection = playerIDs
        /// nextSection = playerSelections
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SyncData Decode(byte[] data)
        {

            if (data.Length == 0)
            {
                Debug.LogError("Invalid data length, unable to process for sync loadout");
            }
            
            var decodedData = new SyncData();

            decodedData.PlayerCount = data[0];

            decodedData.PlayerIDs = new int[decodedData.PlayerCount];
            decodedData.PlayerSelections = new int[decodedData.PlayerCount];
            
            for (int i = 0; i < decodedData.PlayerCount; i++)
            {
                decodedData.PlayerIDs[i] = data[i + 1];
                decodedData.PlayerSelections[i] = data[decodedData.PlayerCount + i + 1];
            }

            return decodedData;
        }

        
        public static byte[] Encode(int playerCount, int[] playerIDArr, int[] playerSelectionArr)
        {
            
            byte[] data = new byte[playerCount + playerCount + 1];

            data[0] = (byte) playerCount;

            for (int i = 0; i < playerCount; i++)
            {
                data[i + 1] = (byte) playerIDArr[i];
                playerSelectionArr[playerCount + i + 1] = (byte) playerSelectionArr[i];
            }

            return data;

        }
        
    }
}