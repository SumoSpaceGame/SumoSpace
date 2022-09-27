using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using FishNet.Connection;
using FishNet.Object;
using Game.Common.Networking;
using Game.Common.Phases;
using Phase = Game.Common.Phases.Phase;

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
