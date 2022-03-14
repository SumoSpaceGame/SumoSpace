using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine.SceneManagement;

namespace Game.Client.Phases
{
    public class ClientPhaseStartMatch : IGamePhase
    {
        public GamePhaseNetworkManager _gamePhaseNetworkManager;

        public ClientPhaseStartMatch(GamePhaseNetworkManager _gamePhaseNetworkManager)
        {
            this._gamePhaseNetworkManager = _gamePhaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            
        }

        private bool sentReady = false;

        public void PhaseUpdate()
        {
            if (!sentReady && SceneManager.GetActiveScene().name == _gamePhaseNetworkManager.masterSettings.matchSettings.SelectedMap)
            {
                sentReady = true;
                _gamePhaseNetworkManager.SendPhaseUpdate(Phase.MATCH_START_COUNTDOWN, new byte[] {1});
            }
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
        }
    }
}