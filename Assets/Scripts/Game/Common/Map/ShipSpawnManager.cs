using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Map
{
    public class ShipSpawnManager : MonoBehaviour
    {
        public struct SpawnGroup
        {
            public Vector3[] SpawnPoints;
            public bool useRespawnPoint;
            public Vector3 RespawnPoint;
            
        }
        public List<SpawnGroup> spawnGroups = new List<SpawnGroup>();


        public Vector3 GetSpawnPoint(int teamIndex, int playerTeamIndex)
        {
            if (teamIndex > spawnGroups.Count)
            {
                teamIndex = spawnGroups.Count - 1;
                Debug.Log("Team index greater than spawn groups, defaulting to last group");
            }

            if (playerTeamIndex > spawnGroups[teamIndex].SpawnPoints.Length)
            {
                playerTeamIndex = spawnGroups[teamIndex].SpawnPoints.Length - 1;
                Debug.Log("Player team index greater than spawn groups, defaulting to last index");
            }

            return spawnGroups[teamIndex].SpawnPoints[playerTeamIndex];
        }

        public Vector3 GetRespawnPoint(int teamIndex, int playerTeamIndex)
        {
            if (teamIndex > spawnGroups.Count)
            {
                teamIndex = spawnGroups.Count - 1;
                Debug.Log("Team index greater than spawn groups, defaulting to last group");
            }

            if (spawnGroups[teamIndex].useRespawnPoint)
            {
                return spawnGroups[teamIndex].RespawnPoint;
            }
            else
            {
                return GetSpawnPoint(teamIndex, playerTeamIndex);
            }
        }
        
        
    }
}