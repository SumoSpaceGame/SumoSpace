using System;
using FishNet;
using FishNet.Object;
using Game.Common.Instances;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Common.Networking.Entity
{
    /// <summary>
    /// If you need to disable and re-enable the object,
    /// Do it under this gameobject.
    /// </summary>
    public abstract class EntityNetworkObject : NetworkBehaviour
    {
        // Empty definer that will be used for visibility later on
        // List<Ships> PresentShips
        // Maximum range
        // The ability to send data to only present ships

        public float Range;

        public UnityEvent OnSpawn;
        public UnityEvent OnDespawn;

        public bool Initialized { get; private set; }
        private bool Destroyed = false;
        public NetworkObject networkObject { get; private set; }
        public int instanceID { get; private set; }
        
        /// <summary>
        /// Should only be called by the EntityNetworkManager
        ///
        /// Initiates the object with values to keep track of for pooling
        /// </summary>
        /// <param name="instanceID"></param>
        public void Init(int instanceID)
        {
            networkObject = this.GetComponent<NetworkObject>();
            Initialized = true;
            Destroyed = false;
            this.instanceID = instanceID;
        }

        public void Awake()
        {
            Destroyed = false;
            OnSpawn?.Invoke();

            if (Initialized)
            {
                
            }
        }

        private void OnDestroy()
        {
            if(!Destroyed) DestroyEntity();
        }
        
        /// <summary>
        /// Should only be called by the EntityNetworkManager
        /// 
        /// Call to destroy this entity, from server side.
        /// Will destroy it on server and client
        /// </summary>
        public void DestroyEntity()
        {
            if (!InstanceFinder.IsServer)
            {
                Debug.LogError("Can not spawn entity on client, has to be from server");
                return;
            }
            

            OnDespawn?.Invoke();
            
            // Destruction and pooling happen automatically
            InstanceFinder.ServerManager.Despawn(networkObject);
        }
        
    }
}