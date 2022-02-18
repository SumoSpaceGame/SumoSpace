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
            
            
                // Register the player, this only happens in the server side.
                masterSettings.playerIDRegistry.RegisterPlayer(player.NetworkId);
                var playerID = masterSettings.playerIDRegistry.Get(player.NetworkId);
                masterSettings.playerStaticDataRegistry.Add(playerID, new PlayerStaticData()
                {
                    // TODO: FIX
                    OwnerID = gameMatchSettings.ClientMatchID,
                    PlayerMatchID = (ushort)player.NetworkId,
                    NetworkID = player.NetworkId
                });
            
                // Create player data
                masterSettings.playerGameDataRegistry.Add(playerID, new PlayerGameData());
                
            
                networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());
            });
        }
        
    }
}
