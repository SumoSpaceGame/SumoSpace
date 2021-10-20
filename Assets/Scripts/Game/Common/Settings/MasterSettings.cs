using System;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Settings
{
    [CreateAssetMenu(fileName = "Settings" ,menuName = "Game Registry/Master Settings", order = 0)]
    public class MasterSettings : ScriptableObject
    {
        [Header("Main settings")]
        public GameMatchSettings matchSettings;
        
        public NetworkSettings network;
        
        
        [Space(4)]
        [Header("Registries")]
        public PlayerShips playerShips;
        
        public PlayerStaticDataRegistry playerStaticDataRegistry;
        
        public PlayerIDRegistry playerIDRegistry;

        public PlayerGameDataRegistry playerGameDataRegistry;
        
        [Space(4)]
        [Header("Misc")]
        public ShipPrefabList ShipPrefabList;
        public void Reset()
        {
            if(playerShips != null) playerShips.Reset();
        }

        public ShipManager GetShip(uint networkID)
        {
            if (playerStaticDataRegistry.TryGet(playerIDRegistry.Get(networkID), out var data))
            {
                if (playerShips.TryGet(data.PlayerMatchID, out ShipManager manager))
                {
                    return manager;
                }
            } 
            
            return null;
        }



    }
}
