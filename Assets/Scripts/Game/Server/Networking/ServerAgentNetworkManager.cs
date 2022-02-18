using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentNetworkManager : AgentManagerBehavior, IGamePersistantInstance
    {
        /// <summary>
        /// Server creation of the ships. This sets up the ships to work authoritivaly with the server.
        /// </summary>
        /// <param name="ClientMatchID"></param>
        partial void ServerCreateShip(PlayerStaticData data)
        {
            var ClientMatchID = data.PlayerMatchID;
            var spawnedShip = _shipSpawner.SpawnShip(data.PlayerMatchID, 0, false);
            
            _playerShips.Add(ClientMatchID, spawnedShip);
    
            var agentMovement = NetworkManager.Instance.InstantiateAgentMovement();
            var agentMovementScript = agentMovement.gameObject.GetComponent<AgentMovementNetworkManager>();
            agentMovementScript.attachedShipManager = spawnedShip;

            spawnedShip.networkMovement = agentMovementScript;
            spawnedShip.isServer = true;
            
            spawnedShip.playerMatchID = data.PlayerMatchID;
            
            var agentInput = (AgentInputManager) NetworkManager.Instance.InstantiateAgentInput();
            
            agentInput.ServerSendOwnership(networkObject.Networker.FindPlayer(player => data.NetworkID == player.NetworkId));
            
            agentInput._shipManager = spawnedShip;
        }

    }
}
