using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Networking.Misc;
using Game.Common.Phases;
using UnityEngine;

namespace Game.Common.Util
{
    public class PersistantUtility
    {
        public delegate void DestroyEvent(Object obj);
        public static void InstantiateNetworkPersistant()
        {
            NetworkManager.Instance.InstantiateMatchTimer();
            NetworkManager.Instance.InstantiateGameManager();
            NetworkManager.Instance.InstantiateAgentManager();
            NetworkManager.Instance.InstantiateInputLayer();
            NetworkManager.Instance.InstantiateGamePhase();
        }
        
        // TODO: Move destruction to self NetworkPersistantInstance, so all network persistants can be cleaned up automatically.

        public static void DestroyNetworkPersistant(DestroyEvent destroyEvent)
        {
            destroyEvent?.Invoke(MainPersistantInstances.Get<MatchNetworkTimerManager>().gameObject);
            destroyEvent?.Invoke(MainPersistantInstances.Get<GameNetworkManager>().gameObject);
            destroyEvent?.Invoke(MainPersistantInstances.Get<AgentNetworkManager>().gameObject);
            destroyEvent?.Invoke(MainPersistantInstances.Get<InputLayerNetworkManager>().gameObject);
            destroyEvent?.Invoke(MainPersistantInstances.Get<GamePhaseNetworkManager>().gameObject);
        }
    }
}