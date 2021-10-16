using System;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName = "PlayerShipsRegistry", menuName = "Game/Player Ships Registry")]
    public class PlayerShips : ScriptableObject
    {
        private void Awake()
        {
            Reset();
        }

        private Dictionary<ushort, ShipManager> _playerShips = new Dictionary<ushort, ShipManager>();

        public void Add(ushort shipID, ShipManager shipManager)
        {
            _playerShips.Add(shipID, shipManager);
        }

        public ShipManager Get(ushort shipID)
        {
            return _playerShips[shipID];
        }

        public void Reset()
        {
            _playerShips.Clear();
        }
    }
}