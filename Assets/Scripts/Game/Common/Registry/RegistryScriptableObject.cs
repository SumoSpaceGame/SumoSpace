using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Registry
{
    public abstract class RegistryScriptableObject<K, T> : ScriptableObject
    {

        public const string REGISTRY_MENU_NAME = "Game Registry/";
        
        private Dictionary<K, T> _registryDictionary = new Dictionary<K, T>();


        public virtual bool Add(K key, T value)
        {
            if (_registryDictionary.ContainsKey(key))
            {
                return false;
            }
            
            //Debug.Log("Adding " + value + " with key " + key);
            
            _registryDictionary.Add(key, value);
            return true;
        }

        public virtual void Remove(K key)
        {
            _registryDictionary.Remove(key);
        }

        public virtual bool Has(K key)
        {
            return _registryDictionary.ContainsKey(key);
        }


        public virtual bool TryGet(K key, out T value)
        {
            return _registryDictionary.TryGetValue(key, out value);
        }
        
        public void Reset()
        {
            _registryDictionary.Clear();
        }

        public virtual T Get(K key)
        {
            return _registryDictionary[key];
        }


        public Dictionary<K, T>.ValueCollection GetAll()
        {
            return _registryDictionary.Values;
        }
    }
}