using BeardedManStudios.Forge.Networking;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class GameManagerNetworkObject : NetworkObject
    {
        protected override bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            return base.ServerAllowRpc(methodId, receivers, args);
        }
        
        
        protected override bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            return false;
        }
    }
}
