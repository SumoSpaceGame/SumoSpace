using System;
using System.Collections.Generic;
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

        private Dictionary<ushort, Ship> _playerShips = new Dictionary<ushort, Ship>();

        public void Add(ushort shipID, Ship ship)
        {
            _playerShips.Add(shipID, ship);
        }

        public Ship Get(ushort shipID)
        {
            return _playerShips[shipID];
        }

        public void Reset()
        {
            _playerShips.Clear();
        }
    }
}