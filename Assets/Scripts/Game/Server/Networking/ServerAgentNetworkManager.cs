using FishNet.Object;
using Game.Common.Instances;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        /// <summary>
        /// Server creation of the ships. This sets up the ships to work authoritivaly with the server.
        /// </summary>
        /// <param name="ClientMatchID"></param>
        partial void ServerCreateShip(PlayerStaticData data)
        {
            PlayerGameData gameData = masterSettings.playerGameDataRegistry.Get(data.GlobalID);

            var shipID = 0;
            if (gameData != null)
            {
                shipID = gameData.shipCreationData.shipType;
            }

            var ClientMatchID = data.GlobalID;
            var spawnedShip = _shipSpawner.SpawnShip(ClientMatchID, shipID, false, false);
            
            _playerShips.Add(ClientMatchID, spawnedShip);
    
            var agentMovement = MainPersistantInstances.Get<NetworkCreator>().InstantiateAgentMovementNetworkManager();
            
            var agentMovementScript = agentMovement.gameObject.GetComponent<AgentMovementNetworkManager>();
            agentMovementScript.attachedShipManager = spawnedShip;

            spawnedShip.networkMovement = agentMovementScript;
            spawnedShip.isServer = true;
            
            spawnedShip.playerMatchID = ClientMatchID;
            
            var agentInput = MainPersistantInstances.Get<NetworkCreator>().InstantiateAgentInputNetworkManager();
            
            agentInput.ServerSendOwnership(ServerManager.Clients[data.GlobalID.NetworkID]);
            
            agentInput._shipManager = spawnedShip;
        }

    }
}
