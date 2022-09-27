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

        public Vector3 InputDirection;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();


            InstanceFinder.TimeManager.OnTick += Tick;
        }


        private void Tick()
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
                SetServerInput(InputDirection);
            }

            if (InstanceFinder.IsServer)
            {   
                var inputDir = InputDirection;
                _shipManager.shipController.targetAngle = inputDir.z;
                _shipManager.shipController.movementVector = new Vector2(inputDir.x, inputDir.y);
            }
        }

        [ServerRpc]
        public void SetServerInput(Vector3 input)
        {
            InputDirection = input;
        }
    }
}