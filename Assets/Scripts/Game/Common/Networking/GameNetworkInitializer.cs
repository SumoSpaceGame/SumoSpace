using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Game.Common.Networking
{
    public class GameNetworkInitializer : MonoBehaviour
    {
        public List<NetworkObject> ManagerPrefabs = new List<NetworkObject>(); 
        
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;
        }
        
        private void OnDestroy()
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;
        }

        public void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                foreach (var obj in ManagerPrefabs)
                {
                    // They will automatically get cleaned up on disconnect
                    var go = Instantiate(obj);
                    InstanceFinder.ServerManager.Spawn(go, null);
                }
            }

        }
    }
}
