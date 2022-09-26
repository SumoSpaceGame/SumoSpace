using System;
using System.Text.RegularExpressions;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Phases;
using Game.Common.Registry;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    {


        partial void OnServerNetworkStart()
        {
            gameMatchSettings.MatchStarted = false;
            networkObject.Networker.playerAccepted += ServerOnPlayerConnected ;
            networkObject.Networker.playerDisconnected += ServerOnPlayerDisconnected;
            networkObject.Networker.playerTimeout += ServerOnPlayerDisconnected;
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
            // Nothing should be done, the client should manually request to join the server first
            // TODO: Add a time out, if they do not request to join in x amount of time disconnect them/timeout
        }

        public void ServerOnPlayerDisconnected(NetworkingPlayer player, NetWorker sender)
        {
            // Make sure the player has an effect on the server if they leave
            if (gameMatchSettings.ServerRestartOnLeave)
            {
                if (masterSettings.playerIDRegistry.TryGetByNetworkID(player.NetworkId, out var id))
                {
                    ResetServer();
                }
                
            }
        }
        
        /// <summary>
        /// Updates game tokens
        ///
        /// A bad design/name, should be replaced later
        /// </summary>
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

        
        public void ResetServer()
        {
            masterSettings.Reset();
            // TODO: Reseting means the unity instance stops and has to spin back up
            // This is the safest approach to prevent waste, and insures a cleaner approach
            // Probably not the best, but its fine.. for now
            // Also add a new systemctl to start the unity process once if none is detected.
            Application.Quit();
        }

        partial void ServerRecieveClientID(NetworkingPlayer player, string id)
        {
            // TODO: Temporary
        }


        partial void ServerRecieveClientJoinRequest(NetworkingPlayer player, string requestingClientID)
        {
            // TODO: Account system EVENTUALLY USE IUserAuthenticator TO AUTHENTICATE IF THE PLAYER IS A LOGGED IN USE
            MainThreadManager.Run(() =>
            {
                if (gameMatchSettings.MatchStarted)
                {
                    if (!gameMatchSettings.AllowSpectators)
                    {
                        player.Networker.Disconnect(false);
                        return;
                    }

                    if (masterSettings.playerIDRegistry.HasClientID(requestingClientID))
                    {
                        PlayerID requestingPlayerID = masterSettings.playerIDRegistry.GetByClientID(requestingClientID);
                        masterSettings.playerIDRegistry.UpdatePlayerIDNetworkID(requestingPlayerID.MatchID, player.NetworkId);
                        // TODO: Send all players with the updated networkID, can be an rpcs 
                        
                        // TODO: Reconnect system
                        // Tell the player to rejoin at this point
                        // Update playerID with the new networkID (myPlayerID in networkObjects)
                    }
                    else
                    {
                        // TODO: Matches will not be open to the public for spectating in the future
                        // For now spectators will be open, but should be registered in the server before hand
                        // Spectators will be for casting, or just game debugging
                        gameMatchSettings.ClientIsSpectator = true;
                    }
                    
                    
                    networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());

                    return;
                }
                
                // If this is a player that is going to play
                // TODO: Add a check against the register players for this server
                
                UpdateGameServerClientSettings();
                // Register the player, this only happens in the server side.
                var id = masterSettings.RegisterPlayer(player.NetworkId, gameMatchSettings.ClientMatchID, requestingClientID);

                if (masterSettings.playerStaticDataRegistry.TryGet(id, out var data))
                {
                    data.TeamID = gameMatchSettings.ClientTeam;
                    data.TeamPosition = gameMatchSettings.ClientTeamPosition;
                }
                gameMatchSettings.ClientIsSpectator = false;
                
                
                networkObject.SendRpc(player, RPC_SYNC_MATCH_SETTINGS, gameMatchSettings.GetSerialized());
            });
        }
    }
}
