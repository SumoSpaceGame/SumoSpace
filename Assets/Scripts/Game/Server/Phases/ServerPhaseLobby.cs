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
                    byte flag = data[0];
                    byte selection = data[1];
                    
                    if (flag == PhaseLobby.PLAYER_SELECT_FLAG)
                    {
                        // TODO: Add spam protection
                        // TODO: Use pure synced client identifier. An identified that is defined by the server.
                        
                        Debug.Log(info.SendingPlayer.NetworkId + " selected in " + selection);
                        
                        _phaseNetworkManager.SendUnreliablePhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_SELECT_FLAG, (byte) info.SendingPlayer.NetworkId, selection});
                    }
                    else if (flag == PhaseLobby.PLAYER_LOCKED_FLAG)
                    {
                        if (_lockedInPlayers.Contains(info.SendingPlayer.NetworkId)) return;
                        
                        _lockedInPlayers.Add(info.SendingPlayer.NetworkId);
                        
                        Debug.Log(info.SendingPlayer.NetworkId + " locked in " + selection);;
                        
                        _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_LOCKED_FLAG, (byte) info.SendingPlayer.NetworkId, (byte) _phaseNetworkManager.masterSettings.playerIDRegistry.Get(info.SendingPlayer.NetworkId).MatchID, selection});


                        if (_phaseNetworkManager.masterSettings.playerGameDataRegistry.TryGet(
                            _phaseNetworkManager.masterSettings.playerIDRegistry.Get(info.SendingPlayer.NetworkId),
                            out var gameData))
                        {
                            gameData.shipCreationData.shipType = selection;
                        }
                        else
                        {
                            Debug.LogError($"Could not find game data for player {info.SendingPlayer.NetworkId}");
                        }
                        
                        if (_lockedInPlayers.Count == _phaseNetworkManager.gameMatchSettings.MaxPlayerCount)
                        {
                            _phaseNetworkManager.ServerNextPhase();
                        }
                    }
                    
                    break;
            }
        }
    }
}