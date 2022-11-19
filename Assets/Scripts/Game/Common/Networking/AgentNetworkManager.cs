using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Game.Common.Instances;
using Game.Common.Registry;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    [RequireComponent(typeof(ShipSpawner))]
    public partial class AgentNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        public GameMatchSettings gameMatchSettings;
        public MasterSettings masterSettings;

        public PlayerShips _playerShips;

        [HideInInspector]
        public ShipSpawner _shipSpawner;
    
        public void Awake()
        {
            _shipSpawner = GetComponent<ShipSpawner>();
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<AgentNetworkManager>();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
            MainPersistantInstances.TryAdd(this);
        }

        /// <summary>
        /// Spawns the ship for the server.
        /// Client ships get spawned when agent movement network manager gets .
        /// </summary>
        /// <param name="clientID"></param>
        public void SpawnShip(PlayerID playerID)
        {
            if (InstanceFinder.IsServer)
            {
                if (masterSettings.playerStaticDataRegistry.TryGet(playerID, out var data))
                {
                    ServerCreateShip(data);
                }
                else
                {
                    Debug.LogError("Tried to spawn a ship for an invalid player");
                }

            }
            else
            {
                Debug.LogError("Tried to call spawn ship while a client. Something is wrong");
            }
        }
        
        partial void ServerCreateShip(PlayerStaticData data);
    }
}
