using System;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName = "PlayerShipsRegistry", menuName = REGISTRY_MENU_NAME + "Player Ships Registry")]
    public class PlayerShips : RegistryScriptableObject<ushort, ShipManager> { }
}