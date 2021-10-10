using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [CreateAssetMenu(fileName = "ShipPrefabList", menuName = "GameSO/ShipPrefabList")]
    public class ShipPrefabList : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> shipPrefabs = new List<GameObject>();

        //TODO: Change when ship design is more situated
        public GameObject GetShip(int shipIndex)
        {
            return shipPrefabs[shipIndex];
        }
    }
}
