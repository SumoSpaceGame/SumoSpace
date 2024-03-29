﻿using System;
using System.Collections.Generic;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Map
{
    public class ShipSpawnManager : MonoBehaviour, IGameInstance
    {
        [Serializable]
        public struct SpawnGroup
        {
            public Transform[] SpawnPoints;
            public bool useRespawnPoint;
            public Transform RespawnPoint;
            
        }
        public List<SpawnGroup> spawnGroups = new List<SpawnGroup>();

        private void Awake()
        {
            MainInstances.Add(this);
        }

        private void OnDestroy()
        {
            MainInstances.Remove<ShipSpawnManager>();
        }

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

            return spawnGroups[teamIndex].SpawnPoints[playerTeamIndex].position;
        }

        public Vector3 GetRespawnPoint(int teamIndex, int playerTeamIndex)
        {
            Debug.Log("Respawn positions " + teamIndex + " " + playerTeamIndex);
            if (teamIndex > spawnGroups.Count)
            {
                teamIndex = spawnGroups.Count - 1;
                Debug.Log("Team index greater than spawn groups, defaulting to last group");
            }

            if (spawnGroups[teamIndex].useRespawnPoint)
            {
                return spawnGroups[teamIndex].RespawnPoint.position;
            }
            else
            {
                return GetSpawnPoint(teamIndex, playerTeamIndex);
            }
        }
        
        
    }
}