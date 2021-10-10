using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    {

        private int serverPlayerCounter = 0;

        partial void OnServerNetworkStart()
        {
                
            networkObject.Networker.playerAccepted += ServerOnPlayerConnected ;
        }
        
        
        
        private void ServerOnPlayerConnected (NetworkingPlayer player, NetWorker sender)
        {
            gameMatchSettings.ClientMatchID = serverPlayerCounter++;
            gameMatchSettings.ClientTeam = (serverPlayerCounter+1) / gameMatchSettings.TeamSize;
            gameMatchSettings.ClientTeamPosition = (serverPlayerCounter+1) % gameMatchSettings.TeamSize;
            
            networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());
        }
        
    }
}
