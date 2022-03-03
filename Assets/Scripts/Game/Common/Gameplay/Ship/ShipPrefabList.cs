using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [CreateAssetMenu(fileName = "ShipPrefabList", menuName = "Game/ShipPrefabList")]
    public class ShipPrefabList : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> shipPrefabs = new List<GameObject>();

        //TODO: Change when ship design is more situated
        public GameObject GetShip(int shipIndex)
        {
            if (shipIndex < 0 || shipIndex >= shipPrefabs.Count)
            {
                Debug.LogError("ShipIndex out of bounds! defaulting to 0");
                return shipPrefabs[0];
            }
            
            return shipPrefabs[shipIndex];
        }
    }
}
