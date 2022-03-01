using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : AgentMovementBehavior
    {

        public PlayerIDRegistry playerIDRegistry;
        
        partial void ClientUpdate()
        {
            attachedShipManager._rigidbody2D.position = networkObject.position;
            attachedShipManager.transform.rotation  = Quaternion.Euler(0,0,networkObject.rotation);
            
        }

        /// <summary>
        /// Spawns the ship in based on server data.
        /// </summary>
        /// <param name="args"></param>
        partial void ClientRequestShipSpawnData(RpcArgs args)
        {
            var data = args.GetAt<string>(0);
            
            var requestData = RequestData.Deserialize(data);
            
            //Spawn ship
            // TODO: Move this code somewhere else
            MainThreadManager.Run(() =>
            {
                var agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();

                var playerID = playerIDRegistry.Get(requestData.clientOwner.ID);
                var playerGameData = agentNetworkManager.masterSettings.playerGameDataRegistry.Get(playerID);
                var ship = agentNetworkManager._shipSpawner.SpawnShip(playerID, playerGameData.shipCreationData.shipType,
                    requestData.clientOwner.ID == networkObject.MyPlayerId);

                masterSettings.playerShips.Add(playerID, ship);
                
                attachedShipManager = ship;
                ship._rigidbody2D.isKinematic = true;
                agentNetworkManager._playerShips.Add(requestData.clientOwner, ship);
                    
                ship.networkMovement = this;
            });

        }
    }
}