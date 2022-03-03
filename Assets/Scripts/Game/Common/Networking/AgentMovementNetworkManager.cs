using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using Game.Common.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common.Networking
{
    
    public partial class AgentMovementNetworkManager : AgentMovementBehavior
    {
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

        public bool initAgentInput = false;
        
        public MasterSettings masterSettings;
        
        
        protected override void NetworkStart()
        {
            base.NetworkStart();
            networkObject.UpdateInterval = masterSettings.network.updateInterval;
            
            if(!networkObject.IsServer) networkObject.SendRpc(RPC_REQUEST_SHIP_SPAWN_DATA, Receivers.Server, "");
        }
    
        
        public void Update()
        {
            if (attachedShipManager == null)
            {
                return;
            }
            if (networkObject.IsServer)
            {
                ServerUpdate();
            }
            else
            {
                ClientUpdate();
            }
        }
        
        
        /// <summary>
        /// Ship spawn data handler, when the client creates a ship, it will ask the server for the ship data to spawn.
        /// </summary>
        /// <param name="args"></param>
        public override void RequestShipSpawnData(RpcArgs args)
        {
            if (networkObject.IsServer)
            {
                ServerRequestShipSpawnData(args);
            }
            else
            {
                ClientRequestShipSpawnData(args);
            }
            
        }

        partial void ClientUpdate();
        partial void ServerUpdate();
        partial void ServerRequestShipSpawnData(RpcArgs args);
        partial void ClientRequestShipSpawnData(RpcArgs args);


    }
    
    
}