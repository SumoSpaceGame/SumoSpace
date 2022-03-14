using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Registry;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    [RequireComponent(typeof(ShipSpawner))]
    public partial class AgentNetworkManager : AgentManagerBehavior, IGamePersistantInstance
    {

        public GameMatchSettings gameMatchSettings;
        public MasterSettings masterSettings;

        public PlayerShips _playerShips;

        [HideInInspector]
        public ShipSpawner _shipSpawner;
    
        public void Awake()
        {
            DontDestroyOnLoad(this);
            _shipSpawner = GetComponent<ShipSpawner>();
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<AgentNetworkManager>();
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });
            
            
        }


        /// <summary>
        /// Spawns the ship for the server.
        /// Client ships get spawned when agent movement network manager gets .
        /// </summary>
        /// <param name="clientID"></param>
        public void SpawnShip(uint clientID)
        {
            if (networkObject.IsServer)
            {
                if (masterSettings.playerStaticDataRegistry.TryGet(masterSettings.playerIDRegistry.Get(clientID), out var data))
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
