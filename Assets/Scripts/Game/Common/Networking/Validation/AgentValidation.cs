

// ReSharper disable CheckNamespace
namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class AgentNetworkObject : NetworkObject
    {
        protected override bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            return newOwner.IsHost;
        }
        
        
        protected override bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            // TODO: IMPORTANT! Make sure the rpc is only being accepted by the pseudo owner of this agent;
            //return args.Info.SendingPlayer.NetworkId ==
            return true;
        }
        
        
        
    }
}
