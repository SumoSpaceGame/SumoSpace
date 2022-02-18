namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class AgentInputNetworkObject : NetworkObject
    {
        public uint ServerPlayerOwnerID;
        
        protected override bool AllowOwnershipChange(NetworkingPlayer newOwner)
        {
            if (newOwner.NetworkId == ServerPlayerOwnerID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}