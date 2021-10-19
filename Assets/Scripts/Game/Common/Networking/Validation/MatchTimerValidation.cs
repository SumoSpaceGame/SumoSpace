namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class MatchTimerNetworkObject : NetworkObject
    {
        protected override bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            return false;
        }

        protected override bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            return false;
        }
    }
}