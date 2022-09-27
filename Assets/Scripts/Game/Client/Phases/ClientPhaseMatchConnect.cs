using System;
using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.UI;
using UnityEngine;

namespace Game.Client.Phases
{
    /// <summary>
    /// Waiting for players to connect. Once all players connect, this will be sent over to the lobby screen.
    /// While this phase is active, it will only be shown as a GUI to the player
    /// </summary>
    public class ClientPhaseMatchConnect : IGamePhase
    {
        private readonly GamePhaseNetworkManager _phaseNetworkManager;
        private readonly MasterUIController _masterUIController;
        private bool _waitingForServer = false;
        
        public ClientPhaseMatchConnect(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
            _masterUIController = MainPersistantInstances.Get<MasterUIController>();
        }
        
        public void PhaseStart()
        {
            _phaseNetworkManager.gameMatchSettings.MatchStarted = false;
            // Enable Animated GUI saying waiting for players
            _masterUIController.ActivateWaitingForPlayer();
        }

        public void PhaseUpdate()
        {
            //Debug.Log("Updating " + _waitingForServer);
            //If this already finished
            if (_waitingForServer) return;   
            
            // Check if the player has pressed the cancelled button, if they did disable GUI and stop match search.
            // If this was matchmaking based, it will signal the match maker that this player has left.
            
            // Wait for network objects to finish initializing and then send over a ready state
            // Use main instances to check if everything important is ready.
            if (IsAllInstantiated())
            {
                this.SendFinished();
            }
        }

        /// <summary>
        /// Checks if all specified game managers are initialized.
        /// </summary>
        /// <returns>If all specified game managers are initialized returns true</returns>
        private bool IsAllInstantiated()
        {
            // TODO: Clean up instantiated if cancelled or disconnected.
            var waitForTypes = PhaseMatchConnect.GAME_NETWORK_INITIALIZE_TYPES;
            bool allInitialized = true;
            
            for (int i = 0; i < waitForTypes.Length; i++)
            {
                Type instanceType = waitForTypes[i];

                if (!MainPersistantInstances.HasType(instanceType))
                {
                    allInitialized = false;
                    break;
                }
            }

            allInitialized &= _phaseNetworkManager.gameMatchSettings.IsDataSynced;

            return allInitialized;
        }
        
        /// <summary>
        /// Sends RPC to server to tell it that this client is ready for the next phase.
        /// </summary>
        private void SendFinished()
        {
            Debug.Log("Finished Instantiating Network");
            
            byte data = new byte();
            data = (byte) PhaseMatchConnect.PLAYER_NETWORK_INITIALIZED_FIN;
                
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_CONNECT, new byte[]{data});
            _waitingForServer = true;
        }

        public void PhaseCleanUp()
        {
            _masterUIController.StopWaitingForPlayer();
            _waitingForServer = false;
        }
        
        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            
            if (data.Length < 1) return;
            
            var statusByte = data[0];
            
            short updateStatus = Convert.ToInt16(statusByte);


            switch (updateStatus)
            {
                case PhaseMatchConnect.MATCH_CANCELLED:
                    
                    break;
            }
            
        }
    }
}