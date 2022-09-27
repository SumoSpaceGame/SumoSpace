using FishNet.Connection;
using FishNet.Object;

namespace Game.Common.Networking
{
    public partial class AgentInputManager : NetworkBehaviour
    {
        
        public void ServerSendOwnership(NetworkConnection target)
        {
            GiveOwnership(target);
        }
        
    }
}