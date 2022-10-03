using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Registry
{
    /// <summary>
    /// Registry for the game data
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerGameDataRegistry", menuName = REGISTRY_MENU_NAME + "Player Game Data Registry")]
    public class PlayerGameDataRegistry : RegistryScriptableObject<PlayerID, PlayerGameData>
    {

        public PlayerGameData Get(PlayerID id)
        {
            PlayerGameData data;
            
            if (!TryGet(id, out data))
            {
                Debug.LogError($"Could not grab game data registry for player {id}");
            }

            return data;
        }
        
    }

    /// <summary>
    /// All data in player game data can be changed.
    /// Do not change to a struct, having a class allows that functionality.
    /// </summary>
    public class PlayerGameData
    {
        public ShipCreationData shipCreationData;
    }
}