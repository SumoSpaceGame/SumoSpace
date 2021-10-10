using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseLobby : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;

        private List<uint> _lockedInPlayers = new List<uint>();
        
        public ServerPhaseLobby(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
        }

        public void PhaseStart()
        {
            //Set selected map
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            if (data.Length == 0) return;

            switch (data.Length)
            {
                case 1:
                    break;
                case 2:

                    if (data[0] == PhaseLobby.PLAYER_SELECT_FLAG)
                    {
                        // TODO: Add spam protection
                        // TODO: Use pure synced client identifier. An identified that is defined by the server.
                        Debug.Log(info.SendingPlayer.NetworkId + " selected in " + data[1]);
                        _phaseNetworkManager.SendUnreliablePhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_SELECT_FLAG, (byte) info.SendingPlayer.NetworkId, data[1]});
                    }
                    else if (data[0] == PhaseLobby.PLAYER_LOCKED_FLAG)
                    {
                        if (_lockedInPlayers.Contains(info.SendingPlayer.NetworkId)) return;
                        
                        _lockedInPlayers.Add(info.SendingPlayer.NetworkId);
                        
                        Debug.Log(info.SendingPlayer.NetworkId + " locked in " + data[1]);;
                        
                        _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_LOCKED_FLAG, (byte) info.SendingPlayer.NetworkId, data[1]});


                        if (_lockedInPlayers.Count == _phaseNetworkManager.gameMatchSettings.PlayerCount)
                        {
                            _phaseNetworkManager.ServerNextPhase();
                        }
                    }
                    
                    break;
            }
        }
    }
}