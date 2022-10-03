using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Networking.NetBible
{

    
    public class Bible : ScriptableObject   
    {
        private Dictionary<string, Object> storage = new Dictionary<string, Object>();
        
        
        public void Set(string key, Object data)
        {
            if (storage.ContainsKey(key))
            {
                storage[key] = data;
            }
            else
            {
                storage.Add(key, data);
            }
        }

        public Object Get(string key)
         {
            //if(storage.TryGetValue(key))
            return null;
         }

    }
}
