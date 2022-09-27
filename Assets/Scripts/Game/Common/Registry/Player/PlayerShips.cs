using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(fileName = "PlayerShipsRegistry", menuName = REGISTRY_MENU_NAME + "Player Ships Registry")]
    public class PlayerShips : RegistryScriptableObject<PlayerID, ShipManager> { }
}