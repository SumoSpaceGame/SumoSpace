using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    
    public partial class AgentMovementNetworkManager : AgentMovementBehavior
    {
        private struct RequestData
        {
            [SerializeField] public ushort clientOwner;

            public string Serialize()
            {
                return JsonUtility.ToJson(this);
            }

            public static RequestData Deserialize(string data)
            {
                return JsonUtility.FromJson<RequestData>(data);
            }
        }
        
        public Ship attachedShip;

        public MasterSettings masterSettings;
        
        
        protected override void NetworkStart()
        {
            networkObject.UpdateInterval = masterSettings.network.updateInterval;
            base.NetworkStart();
            
            if(!networkObject.IsServer) networkObject.SendRpc(RPC_REQUEST_SHIP_SPAWN_DATA, Receivers.Server, "");
        }
    
        
        public void Update()
        {
            if (attachedShip == null)
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
        
        partial void ClientUpdate();
        partial void ServerUpdate();
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

        partial void ServerRequestShipSpawnData(RpcArgs args);
        partial void ClientRequestShipSpawnData(RpcArgs args);
    }
    
    
}