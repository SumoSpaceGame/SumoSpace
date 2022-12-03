using FishNet;
using FishNet.Object;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking
{
    public class NetworkCreator : MonoBehaviour, IGamePersistantInstance
    {
        public NetworkObject agentMovementNetworkManagerPrefab;
        public NetworkObject agentInputNetworkManagerPrefab;
        public NetworkObject agentValueNetworkManagerPrefab;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            MainPersistantInstances.TryAdd(this);
        }

        public AgentMovementNetworkManager InstantiateAgentMovementNetworkManager()
        {
            var go = Instantiate(agentMovementNetworkManagerPrefab);
            InstanceFinder.ServerManager.Spawn(go);
            return go.GetComponent<AgentMovementNetworkManager>();
        }
        public AgentInputManager InstantiateAgentInputNetworkManager()
        {
            var go = Instantiate(agentInputNetworkManagerPrefab);
            InstanceFinder.ServerManager.Spawn(go);
            return go.GetComponent<AgentInputManager>();
        }
    }
}