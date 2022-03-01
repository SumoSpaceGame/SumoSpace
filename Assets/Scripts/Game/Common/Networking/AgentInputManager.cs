using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentInputManager : AgentInputBehavior
    {

        public MasterSettings masterSettings;

        public ShipManager _shipManager;
        private bool isOwner = false;
        protected override void NetworkStart()
        {
            base.NetworkStart();
            this.networkObject.UpdateInterval = masterSettings.network.updateInterval;
        }

        private void Update()
        {
            if (this.networkObject.IsOwner && !networkObject.IsServer)
            {
                if (_shipManager == null)
                {
                    var agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
                    if (agentNetworkManager == null) return;
                    
                    agentNetworkManager._playerShips
                        .TryGet(agentNetworkManager.masterSettings.playerIDRegistry.Get(networkObject.MyPlayerId), out _shipManager);
                    return;
                }
                
                var clientControls = _shipManager.clientControls;
                networkObject.inputRotation = clientControls.movementRotation;
                networkObject.inputDirection = clientControls.movementDirection;
            }

            if (networkObject.IsServer)
            {
                _shipManager.shipController.targetAngle = networkObject.inputRotation;
                _shipManager.shipController.movementVector = networkObject.inputDirection;
            }
        }

        public override void GiveOwnership(RpcArgs args)
        {
            if (!args.Info.SendingPlayer.IsHost)
            {
                return;
            }

            isOwner = true;

            if (_shipManager != null)
            {
                Debug.Log($"Taking control of ship {_shipManager.playerMatchID}");
            }

            networkObject.TakeOwnership();
        }
    }
}