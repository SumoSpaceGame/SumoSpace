using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Object;
using Game.Common.Instances;
using Game.Common.Map;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking.Entity
{
    /// <summary>
    /// Handles the creation and deletion of network entities
    ///
    /// Wraps network spawning, but also allows the alterations in the future like
    /// - Range only entities
    /// 
    /// </summary>
    public class EntityNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {

        private Dictionary<int, Stack<EntityNetworkObject>> pool = new Dictionary<int, Stack<EntityNetworkObject>>();
        private void Awake()
        {
            MainPersistantInstances.Add(this);
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<EntityNetworkManager>();
        }

        private MasterSettings settings;
        
        /// <summary>
        /// Spawns entity from server
        ///
        /// If network object is in pooling mode, it will automatically grab a pooled instance of that network object
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns>Initiated object</returns>
        public EntityNetworkObject SpawnEntity(EntityNetworkObject prefab)
        {
            EntityNetworkObject ret;
            
            if (!InstanceFinder.IsServer)
            {
                Debug.LogError("Can not spawn entity on client, has to be from server");
                return null;
            }

            if (prefab.gameObject.scene.rootCount != 0)
            {
                Debug.LogError("Can only spawn entity with the prefab");
                return null;
            }
            
            
            if (!prefab.Initialized)
                prefab.Init(-1);
            
            if (prefab.networkObject == null)
            {
                Debug.LogError("Failed to get EntityNetworkObject while spawning entity of " + prefab.name);
                return null;
            }
            
            // Grabs instance of a prefab in pool first, if failed it will instantiate it
            if (prefab.networkObject.GetDefaultDespawnType() == DespawnType.Pool)
            {
                // Check if the object is already instatiated in the pool
                if (pool.TryGetValue(prefab.gameObject.GetInstanceID(), out var stack))
                {
                    if (!stack.TryPop(out ret))
                    {
                        ret = Instantiate(prefab);
                    }
                    
                    
                    ret.gameObject.SetActive(true);
                }
                else
                {
                    var list = new Stack<EntityNetworkObject>();
                    list.Push(ret = Instantiate(prefab));
                    pool.Add(prefab.gameObject.GetInstanceID(), new Stack<EntityNetworkObject>());
                }
            }
            else
            {
                ret = Instantiate(prefab);
            }
            
            
            ret.Init(prefab.GetInstanceID());
            InstanceFinder.ServerManager.Spawn(ret.networkObject);
            
            return ret;
        }
        
        /// <summary>
        /// Used to despawn an initiated EntityNetworkObject from the network
        /// </summary>
        /// <param name="obj"></param>
        public void DespawnEntity(EntityNetworkObject obj)
        {
            if (!InstanceFinder.IsServer)
            {
                Debug.LogError("Can not despawn an entity on client, has to be from server");
                return;
            }
            
            if (!obj.Initialized)
                return;

            if (obj.networkObject.GetDefaultDespawnType() == DespawnType.Pool)
            {
                obj.gameObject.SetActive(false);
                AddToPool(obj);
                obj.DestroyEntity();
            }
        }
        
        /// <summary>
        /// Adds an initiated EntityNetworkobject to the pool
        /// </summary>
        /// <param name="obj"></param>
        private void AddToPool(EntityNetworkObject obj)
        {
            if (!obj.Initialized)
            {
                Debug.LogError("Can not add object to pool if not initated");
                return;
            }

            if (pool.TryGetValue(obj.instanceID, out var stack))
            {
                stack.Push(obj);
            }
            else
            {
                stack = new Stack<EntityNetworkObject>();
                stack.Push(obj);
                pool.Add(obj.instanceID, stack);
            }
        }
        
        /// <summary>
        /// Destroys all instances of an object in the pool
        /// </summary>
        public void CleanUp()
        {
            foreach (var list in pool.Values)
            {
                foreach (var elem in list)
                {
                    elem.DestroyEntity();
                }
            }
        }

    }
}