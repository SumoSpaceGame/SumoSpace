using System;
using UnityEngine;

namespace Game.Common.Map
{
    [CreateAssetMenu(fileName = "Map settings", menuName = "Game Registry/Map settings")]
    [Serializable]
    public class MapSettings : ScriptableObject
    {
        [SerializeField]
        public double MatchTimeMinutes = 30;
    }
}