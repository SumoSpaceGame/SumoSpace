using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentInputManager : NetworkBehaviour
    {

        public MasterSettings masterSettings;

        public ShipManager _shipManager;

        [SyncVar(SendRate = 0f, Channel = Channel.Unreliable)]
        public Vector3 InputDirection;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
            
        }


        private void Update()
        {
            if (this.NetworkObject.IsOwner && !InstanceFinder.IsServer)
            {
                if (_shipManager == null)
                {
                    var agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
                    if (agentNetworkManager == null) return;
                    
                    agentNetworkManager._playerShips
                        .TryGet(agentNetworkManager.masterSettings.playerIDRegistry.GetByMatchID(masterSettings.matchSettings.ClientMatchID), out _shipManager);
                    return;
                }
                
                var clientControls = _shipManager.clientControls;
                //networkObject.inputRotation = clientControls.movementRotation;
                InputDirection = new Vector3(clientControls.movementDirection.x,clientControls.movementDirection.y, clientControls.movementRotation);
            }

            if (InstanceFinder.IsServer)
            {   
                var inputDir = InputDirection;
                _shipManager.shipController.targetAngle = inputDir.z;
                _shipManager.shipController.movementVector = new Vector2(inputDir.x, inputDir.y);
            }
        }
    }
}