using System;
using FishNet.Connection;
using FishNet.Object;
using Game.Common.Registry;
using UnityEngine;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : NetworkBehaviour
    {


        partial void OnServerNetworkStart()
        {
            gameMatchSettings.MatchStarted = false;
        }


        private void ServerOnPlayerConnected (NetworkConnection conn)
        {
            // Nothing should be done, the client should manually request to join the server first
            // TODO: Add a time out, if they do not request to join in x amount of time disconnect them/timeout
        }

        public void ServerOnPlayerDisconnected(NetworkConnection conn)
        {
            // Make sure the player has an effect on the server if they leave
            if (gameMatchSettings.ServerRestartOnLeave)
            {
                if (masterSettings.playerIDRegistry.TryGetByNetworkID(conn.ClientId, out var id))
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
            masterSettings.ResetMatchData();
            // TODO: Reseting means the unity instance stops and has to spin back up
            // This is the safest approach to prevent waste, and insures a cleaner approach
            // Probably not the best, but its fine.. for now
            // Also add a new systemctl to start the unity process once if none is detected.
            Application.Quit();
        }



        partial void ServerRecieveClientJoinRequest(NetworkConnection player, string requestingClientID)
        {
            // TODO: Account system EVENTUALLY USE IUserAuthenticator TO AUTHENTICATE IF THE PLAYER IS A LOGGED IN USE
            if (gameMatchSettings.MatchStarted)
            {
                if (!gameMatchSettings.AllowSpectators)
                {
                    player.Disconnect(false);
                    return;
                }

                if (masterSettings.playerIDRegistry.HasClientID(requestingClientID))
                {
                    PlayerID requestingPlayerID = masterSettings.playerIDRegistry.GetByClientID(requestingClientID);
                    masterSettings.playerIDRegistry.UpdatePlayerIDNetworkID(requestingPlayerID.MatchID, player.ClientId);
                    UpdatePlayerNetworkID(player.ClientId, requestingPlayerID.MatchID);
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

                SyncMatchSettings(player, gameMatchSettings.GetSerialized());
                return;
            }
                
            // If this is a player that is going to play
            // TODO: Add a check against the register players for this server
                
            UpdateGameServerClientSettings();
            // Register the player, this only happens in the server side.
            var id = masterSettings.RegisterPlayer(player.ClientId, gameMatchSettings.ClientMatchID, requestingClientID);

            if (masterSettings.playerStaticDataRegistry.TryGet(id, out var data))
            {
                data.TeamID = gameMatchSettings.ClientTeam;
                data.TeamPosition = gameMatchSettings.ClientTeamPosition;
            }
            
            masterSettings.DebugLogPlayerStatic();
            
            gameMatchSettings.ClientIsSpectator = false;
                
            SyncMatchSettings(player, gameMatchSettings.GetSerialized());
        }
    }
}
