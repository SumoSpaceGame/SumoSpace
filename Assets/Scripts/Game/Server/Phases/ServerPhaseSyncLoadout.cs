using System.Collections.Generic;
using FishNet.Connection;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseSyncLoadout : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;

        private Dictionary<int, PlayerID> _syncedPlayer = new Dictionary<int, PlayerID>();
        public ServerPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _phaseNetworkManager = gamePhaseNetworkManager;
        }

        public void PhaseStart()
        {
            PhaseSyncLoadout.Data data = new PhaseSyncLoadout.Data();

            var playerList = new List<ushort>();
            var selectionList = new List<int>();

            var players = _phaseNetworkManager.masterSettings.GetPlayerIDs();

            foreach (var id in players)
            {
                playerList.Add(id.MatchID);
                
                selectionList.Add(_phaseNetworkManager.masterSettings.playerGameDataRegistry.Get(id).shipCreationData.shipType);
            }

            data.PlayerSelections = selectionList.ToArray();
            data.PlayerIDs = playerList.ToArray();
            
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_SYNC_LOAD_OUTS, PhaseSyncLoadout.Serialized(data));
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            PlayerID playerID;
            if (!_phaseNetworkManager.masterSettings.playerIDRegistry.TryGetByNetworkID(conn.ClientId,
                    out playerID))
            {
                Debug.LogWarning("WARNING: Phase data recieved from client that is not registered? Spectator?");
                return;
            }
            
            _syncedPlayer.Add(conn.ClientId, playerID);

            if (_syncedPlayer.Count == _phaseNetworkManager.gameMatchSettings.MaxPlayerCount)
            {
                _phaseNetworkManager.ServerNextPhase();
            }

        }
    }
}