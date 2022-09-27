
using FishNet.Object;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : NetworkBehaviour
    { 
        
        partial void OnClientNetworkStart()
        {
            RequestServerJoin(this.clientID);
        }

        partial void ClientUpdatePlayerNetworkID(int networkID, ushort matchID)
        {
            masterSettings.playerIDRegistry.UpdatePlayerIDNetworkID(matchID, networkID);
        }
    }
    
}
