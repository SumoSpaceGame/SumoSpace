using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Settings
{
    [CreateAssetMenu(fileName = "Settings" ,menuName = "GameSO/Master Settings", order = 0)]
    public class SettingsScriptableObject : ScriptableObject
    {
        public NetworkSettings network;
    }
}
