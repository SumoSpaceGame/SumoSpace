using System;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Settings
{
    [CreateAssetMenu(fileName = "Settings" ,menuName = "Game/Master Settings", order = 0)]
    public class MasterSettings : ScriptableObject
    {
        public GameMatchSettings matchSettings;
        public NetworkSettings network;
        public PlayerShips playerShips;
        public PlayerDataRegistry playerDataRegistry;
        public PlayerIDRegistry playerIDRegistry;
        public ShipPrefabList ShipPrefabList;
        public void Reset()
        {
            if(playerShips != null) playerShips.Reset();
        }

        public ShipManager GetShip(uint networkID)
        {
            if (playerDataRegistry.TryGet(playerIDRegistry.Get(networkID), out var data))
            {
                return playerShips.Get(data.PlayerMatchID);
            } 
            
            return null;
        }



    }
}
