using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

namespace Game.Common.Networking
{
    public partial class AgentInputManager : AgentInputBehavior
    {
        
        public void ServerSendOwnership(NetworkingPlayer target)
        {
            
            networkObject.ServerPlayerOwnerID = target.NetworkId;
            networkObject.SendRpc(target, RPC_GIVE_OWNERSHIP);
        }
        
    }
}