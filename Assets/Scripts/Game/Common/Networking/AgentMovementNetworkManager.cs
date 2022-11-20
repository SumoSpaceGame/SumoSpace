using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using Game.Common.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Networking
{
    
    public partial class AgentMovementNetworkManager : NetworkBehaviour
    {
        [SyncVar(SendRate = 0)]
        public ushort TempPassiveCharge; // TODO: Switch to better solution.

        /// <summary>
        /// Ship request data. Should contain all information that will spawn a ship.
        /// </summary>
        [Serializable]
        private struct RequestData
        {
            [SerializeField] public PlayerID clientOwner;

            public string Serialize()
            {
                return JsonUtility.ToJson(this);
            }

            public static RequestData Deserialize(string data)
            {
                return JsonUtility.FromJson<RequestData>(data);
            }
        }
        
        
        
        [FormerlySerializedAs("attachedShip")] public ShipManager attachedShipManager;

        public MasterSettings masterSettings;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (InstanceFinder.IsClient)
            {
                ServerRequestShipSpawnData();
            }
        }
        
        
        public void Update()
        {
            if (attachedShipManager == null)
            {
                return;
            }
            if (InstanceFinder.IsServer)
            {
                ServerUpdate();
            }
            else
            {
                ClientUpdate();
            }
        }
        
        

        partial void ClientUpdate();
        partial void ServerUpdate();
        
        [ServerRpc(RequireOwnership = false)]
        partial void ServerRequestShipSpawnData(NetworkConnection conn = null);
        
        [TargetRpc]
        partial void ClientRequestShipSpawnData(NetworkConnection conn, RequestData data);


    }
    
    
}