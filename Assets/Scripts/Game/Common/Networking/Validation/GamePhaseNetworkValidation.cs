using Game.Common.Networking;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class GamePhaseNetworkObject : NetworkObject
    {
        protected override bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            if (methodId == GamePhaseBehavior.RPC_SWITCH_PHASE)
            {
                return false;
            }
            
            
            return base.ServerAllowRpc(methodId, receivers, args);
        }

        protected override bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            return false;
        }
    }
}