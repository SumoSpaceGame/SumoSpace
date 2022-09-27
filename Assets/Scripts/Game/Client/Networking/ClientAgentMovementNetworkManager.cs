using FishNet.Connection;
using FishNet.Object;
using Game.Common.Instances;
using Game.Common.Registry;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : NetworkBehaviour
    {

        public PlayerIDRegistry playerIDRegistry;
        
        partial void ClientUpdate()
        {
            attachedShipManager._rigidbody2D.position = this.transform.position;
            attachedShipManager.transform.rotation  = this.transform.rotation;
        }

        /// <summary>
        /// Spawns the ship in based on server data.
        /// </summary>
        /// <param name="args"></param>
        partial void ClientRequestShipSpawnData(NetworkConnection conn, RequestData data)
        {
            var requestData = data;
            
            var agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();

            var playerID = playerIDRegistry.GetByMatchID(requestData.clientOwner.MatchID);
            var playerGameData = agentNetworkManager.masterSettings.playerGameDataRegistry.Get(playerID);
            var isPlayer = requestData.clientOwner.NetworkID == conn.ClientId;
                
            var isEnemy = false;
            var ship = agentNetworkManager._shipSpawner.SpawnShip(playerID, playerGameData.shipCreationData.shipType,
                isPlayer, isEnemy);

            masterSettings.playerShips.Add(playerID, ship);
                
            attachedShipManager = ship;
            ship._rigidbody2D.isKinematic = true;
            agentNetworkManager._playerShips.Add(requestData.clientOwner, ship);
                    
            ship.networkMovement = this;

        }
    }
}