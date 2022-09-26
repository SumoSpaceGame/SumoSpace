using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Networking;
using Game.Common.Phases;
using Phase = Game.Common.Phases.Phase;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    { 
        partial void OnClientNetworkClose(NetWorker sender)
        {
            
        }
        
        partial void OnClientNetworkStart()
        {
            networkObject.SendRpc(RPC_REQUEST_SERVER_JOIN, Receivers.Server, this.clientID);
        }

        partial void ClientUpdatePlayerNetworkID(uint networkID, ushort matchID)
        {
            masterSettings.playerIDRegistry.UpdatePlayerIDNetworkID(matchID, networkID);
        }
    }
    
}
