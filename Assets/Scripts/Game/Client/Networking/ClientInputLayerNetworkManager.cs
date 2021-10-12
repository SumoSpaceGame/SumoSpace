
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
    {
        
        public void SendMovementUpdate(Vector2 movementVec, float rotation)
        {
            if (networkObject.IsServer)
            {
                Debug.LogError("Send movement update activated on server. This shouldn't happen");
                return;
            }
            networkObject.SendRpcUnreliable(RPC_MOVEMENT_UPDATE, Receivers.Server, movementVec, rotation);
        }
        
        
    }
}
