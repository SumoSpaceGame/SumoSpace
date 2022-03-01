using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Phases;
using UnityEngine;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    {


        partial void OnServerNetworkStart()
        {
            networkObject.Networker.playerAccepted += ServerOnPlayerConnected ;
        }

        partial void OnServerNetworkClose(NetWorker sender)
        {
            Debug.LogError("SERVER SOCKET DISCONNECTED!");
        }

        /// <summary>
        /// If a player is disconnects, check the current phase, and if its in a recoverable phase executes
        ///
        /// Match ready up -> clean up player (should stop the server)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sender"></param>
        partial void OnServerNetworkPlayerDisconnected(NetworkingPlayer player, NetWorker sender)
        {
            
            var phaseManager = MainPersistantInstances.Get<GamePhaseNetworkManager>();
            MainThreadManager.Run(() =>
            {
                UpdateGameServerClientSettings();

            });

            switch (phaseManager.CurrentPhase)
            {
                case Phase.MATCH_READY_UP:
                    // TODO: Stop the server if this is the case, along with lobby, and other states that are critical
                    masterSettings.CleanupPlayer(player.NetworkId);
                    break;
                case Phase.MATCH_GAME:
                    // TODO: Implement reconnection
                    break;
                default:
                    // TODO: Re-enable after creating proper client back to main menu
                    //Application.Quit();
                    break;
            }
        }
        
        private void ServerOnPlayerConnected (NetworkingPlayer player, NetWorker sender)
        {
            MainThreadManager.Run(() =>
            {
                UpdateGameServerClientSettings();
            
            
                // Register the player, this only happens in the server side.
                masterSettings.RegisterPlayer(player.NetworkId, gameMatchSettings.ClientMatchID, "");
                
                networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());
            });
        }

        private void UpdateGameServerClientSettings()
        {
            //Starts at 0
            var currentMatchID = masterSettings.GetPlayerCount();
            
            gameMatchSettings.ClientMatchID = Convert.ToUInt16(currentMatchID);
            
            // TODO: Convert client team system to a more defined design when final execution occurs
            // Right now this is a temp solution
            
            gameMatchSettings.ClientTeam = currentMatchID / gameMatchSettings.TeamSize;
            gameMatchSettings.ClientTeamPosition = currentMatchID % gameMatchSettings.TeamSize;
        }
        
    }
}
