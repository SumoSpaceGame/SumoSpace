using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using UnityEngine;

namespace Game.Server.Phases
{
    /// <summary>
    /// Wait for all players to connect and reach max server.
    /// Once all players connect send to ready up phase
    /// </summary>
    public class ServerPhaseMatchConnect : IGamePhase
    {
        
        


        private readonly GamePhaseNetworkManager _phaseNetworkManager;

        // All variables to cleanup
        private List<uint> readyPlayers = new List<uint>();
        private bool readyForNext = false;
        public ServerPhaseMatchConnect(GamePhaseNetworkManager phaseNetworkManager)
        {
            this._phaseNetworkManager = phaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            
        }

        public void PhaseUpdate()
        {
            //Wait until everyone is ready to start communications
            //If so send everyone to the ready up screen
            
            // TODO: Create a global location to store game match data. Replace this with that
            
            
            
            //Also set timer to 10 seconds. If not everyone is ready by then cancel match.
        }

        public bool PhaseWantsNext()
        {
            return readyForNext;
        }

        public bool PhaseWantsSwitch(out Phase phase)
        {
            phase = Phase.MATCH_CONNECT;
            return false;
        }


        public void PhaseCleanUp()
        {
            readyForNext = false;
            readyPlayers.Clear();
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            
            if (data.Length < 1) return;
            
            var statusByte = data[0];
            
            short updateStatus = Convert.ToInt16(statusByte);

            switch (updateStatus)
            {
                case PhaseMatchConnect.PLAYER_NETWORK_INITIALIZED_FIN:
                    if (!readyPlayers.Contains(info.SendingPlayer.NetworkId))
                    {
                        Debug.Log(info.SendingPlayer.Name + " connected");
                        readyPlayers.Add(info.SendingPlayer.NetworkId);
                    }
                    
                    
                    if (readyPlayers.Count == GameNetworkManager.MATCH_PLAYER_SIZE)
                    {
                        readyForNext = true;
                    }
                    break;
            }
        }
    }
}
