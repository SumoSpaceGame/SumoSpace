using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;
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

            PlayerID playerID;

            if (!_phaseNetworkManager.masterSettings.playerIDRegistry.TryGetByNetworkID(info.SendingPlayer.NetworkId, out playerID))
            {
                Debug.LogWarning("Non-registered player tried to join the network! " + info.SendingPlayer.Ip);
                return;
            }
            
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
                        
                        Debug.Log(playerID.ClientID + " selected in " + selection);
                        
                        _phaseNetworkManager.SendUnreliablePhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_SELECT_FLAG, (byte) playerID.MatchID, selection});
                    }
                    else if (flag == PhaseLobby.PLAYER_LOCKED_FLAG)
                    {
                        if (_lockedInPlayers.Contains(playerID.MatchID)) return;
                        
                        _lockedInPlayers.Add(playerID.MatchID);
                        
                        Debug.Log(playerID.MatchID + " locked in " + selection);;
                        
                        _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOBBY, 
                            new []{(byte) PhaseLobby.PLAYER_LOCKED_FLAG, (byte) playerID.MatchID, (byte) playerID.MatchID, selection});


                        if (_phaseNetworkManager.masterSettings.playerGameDataRegistry.TryGet(playerID,
                            out var gameData))
                        {
                            gameData.shipCreationData.shipType = selection;
                        }
                        else
                        {
                            Debug.LogError($"Could not find game data for player {playerID.ClientID}");
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