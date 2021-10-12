using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Registry;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    {

        private ushort serverPlayerCounter = 0;

        partial void OnServerNetworkStart()
        {
                
            networkObject.Networker.playerAccepted += ServerOnPlayerConnected ;
        }
        
        
        
        private void ServerOnPlayerConnected (NetworkingPlayer player, NetWorker sender)
        {
            MainThreadManager.Run(() =>
            {
                // TODO: Replace serverPlayerCounter to client ID. Map clientMatchID to accountID (Account will be create soon)
                gameMatchSettings.ClientMatchID = serverPlayerCounter++;
                gameMatchSettings.ClientTeam = (serverPlayerCounter+1) / gameMatchSettings.TeamSize;
                gameMatchSettings.ClientTeamPosition = (serverPlayerCounter+1) % gameMatchSettings.TeamSize;
            
            
                var playerID = masterSettings.playerIDRegistry.RegisterPlayer(player.NetworkId);
                masterSettings.playerDataRegistry.Add(playerID, new PlayerData()
                {
                    PlayerMatchID = gameMatchSettings.ClientMatchID
                });
            
            
            
                networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());
            });
        }
        
    }
}
