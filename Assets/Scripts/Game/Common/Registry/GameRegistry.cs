using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    [CreateAssetMenu(menuName = "Game/Game Registry", fileName = "GameRegistry")]
    public class GameRegistry : ScriptableObject
    {

        private Dictionary<string, int> _registry = new Dictionary<string, int>();
        
        public void Register(string key, int value)
        {
            _registry.Add(key, value);
        }

        public void Deregister(string key)
        {
            _registry.Remove(key);
        }

        public bool TryGet(string key, out int value)
        {
            
            if (_registry.TryGetValue(key, out value))
            {
                return true;
            }
            
            return false;
        }
        
        
    }
}
