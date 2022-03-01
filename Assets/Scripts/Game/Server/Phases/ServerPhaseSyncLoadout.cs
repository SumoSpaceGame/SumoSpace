using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
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

        private Dictionary<uint, PlayerID> _syncedPlayer = new Dictionary<uint, PlayerID>();
        public ServerPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _phaseNetworkManager = gamePhaseNetworkManager;
        }

        public void PhaseStart()
        {
            PhaseSyncLoadout.Data data = new PhaseSyncLoadout.Data();

            var playerList = new List<uint>();
            var selectionList = new List<int>();

            var players = _phaseNetworkManager.masterSettings.playerIDRegistry.GetPlayers();

            foreach (var id in players)
            {
                playerList.Add(id.ID);
                
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

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            _syncedPlayer.Add(info.SendingPlayer.NetworkId, _phaseNetworkManager.masterSettings.playerIDRegistry.Get(info.SendingPlayer.NetworkId));

            if (_syncedPlayer.Count == _phaseNetworkManager.gameMatchSettings.PlayerCount)
            {
                _phaseNetworkManager.ServerNextPhase();
            }

        }
    }
}