using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Common.Map
{
    [CreateAssetMenu(fileName = "Map Registry", menuName = "Game Registry/Maps Regsitry")]
    public class MapRegistry : ScriptableObject
    {
        [Serializable]
        public class MapItem
        {
            // If you want to make a new gamemode for the same map, most likely copy and paste the map and 
            // add the game mode specific elements
            // Maps can be made with prefabs if that helps, or done with additive scenes
            // To be determined
            [SerializeField] public MapSettings mapSettings;
            [SerializeField] public string sceneName;
            [HideInInspector] public int index;
            
            public Scene GetScene()
            {
                return SceneManager.GetSceneByName(sceneName);
            }
        }
        
        [SerializeField]
        private List<MapItem> mapItems = new List<MapItem>();

        private void OnValidate()
        {
            for(int i = mapItems.Count - 1; i >= 0; i--)
            {
                var mapItem = mapItems[i];
                if (!SceneManager.GetSceneByName(mapItem.sceneName).IsValid())
                {
                    Debug.Log("Invalid scene in map registry " + mapItem.sceneName);
                }
                
                mapItem.index = i;
            }
        }
        
        /// <summary>
        /// Remove invalid map scenes from map item
        /// </summary>
        public void CleanUpList()
        {
            for(int i = mapItems.Count - 1; i >= 0; i--)
            {
                var mapItem = mapItems[i];
                
                if (!SceneManager.GetSceneByName(mapItem.sceneName).IsValid())
                {
                    Debug.Log("Deregistering map from map registry, invalid map scene " + mapItem.sceneName);
                    mapItems.RemoveAt(i);    
                }
            }
        }
        
        /// <summary>
        /// Add map item if no conflicts are found
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddMapItem(MapItem item)
        {
            if (!SceneManager.GetSceneByName(item.sceneName).IsValid())
            {
                Debug.Log("Invalid scene could not add to map registry " + item.sceneName);
                return false;
            }
            
            foreach (var mapItem in mapItems)
            {
                if (mapItem.sceneName == item.sceneName)
                {
                    Debug.LogError("Found duplicate scene in map registry, could not add.");
                    return false;
                }
            }
            
            mapItems.Add(item);
            
            OnValidate();
            
            return true;
        }
        
        /// <summary>
        /// Get map item by name.
        ///
        /// Maps are stored by name (each map should have a unique name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MapItem GetMap(string name)
        {
            foreach (var map in mapItems)
            {
                if (map.sceneName == name)
                {
                    return map;
                }
            }
            
            Debug.LogWarning("Failed to get map from map registry by string " + name);
            return null;
        }
        
        /// <summary>
        /// If index is known, this is the faster way to grab information. Usually not needed 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MapItem GetMap(int index)
        {
            if (mapItems.Count > index && mapItems.Count != 0)
            {
                return mapItems[index];
            }
        
            Debug.LogWarning("Tried to get map by index out of range " + index);
            return null;
        }

        
        /// <summary>
        /// Check if map exists by name
        ///
        /// Used with parsing args, but can be used for other items
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasMap(string name)
        {
            foreach (var map in mapItems)
            {
                if (map.sceneName == name)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Check if index is in range
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasMap(int index)
        {
            if (mapItems.Count > index && mapItems.Count != 0)
            {
                return true;
            }
            return false;
        }
        
        
    }
}