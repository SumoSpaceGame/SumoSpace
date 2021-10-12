using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentNetworkManager : AgentManagerBehavior, IGamePersistantInstance
    {
        /// <summary>
        /// Server creation of the ships. This sets up the ships to work authoritivaly with the server.
        /// </summary>
        /// <param name="ClientMatchID"></param>
        partial void ServerCreateShip(ushort ClientMatchID)
        {
            var spawnedShip = _shipSpawner.SpawnShip(ClientMatchID, 0, false);
    
            _playerShips.Add(ClientMatchID, spawnedShip);
    
            var agentMovement = NetworkManager.Instance.InstantiateAgentMovement();
            var agentMovementScript = agentMovement.gameObject.GetComponent<AgentMovementNetworkManager>();
            agentMovementScript.attachedShip = spawnedShip;

            spawnedShip.networkMovement = agentMovementScript;
            spawnedShip.isServer = true;
        }
    }
}
