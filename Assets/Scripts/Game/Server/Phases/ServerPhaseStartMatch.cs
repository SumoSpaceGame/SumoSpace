using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Networking.Misc;
using Game.Common.Phases;

namespace Game.Server.Phases
{
    public class ServerPhaseStartMatch : IGamePhase
    {

        private GamePhaseNetworkManager _gamePhaseNetworkManager;
        private MatchNetworkTimerManager _matchNetworkTimerManager;

        private MatchNetworkTimer timer;
        public ServerPhaseStartMatch(GamePhaseNetworkManager gamePhaseNetworkManager, MatchNetworkTimerManager matchNetworkTimerManager)
        {
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
            _matchNetworkTimerManager = matchNetworkTimerManager;
        }
        
        public void PhaseStart()
        {
            timer = _matchNetworkTimerManager.CreateTimer();
            
            //Send to clients to sync timer
            
            timer.StartTimer(30 * 1000);
            timer.StopEvent += OnTimerFinished;
        }

        public void PhaseUpdate()
        {
            
        }

        public void PhaseCleanUp()
        {
            timer = null;
        }

        public void OnTimerFinished()
        {
            _gamePhaseNetworkManager.ServerNextPhase();
            if(timer != null) timer.StopEvent -= OnTimerFinished;
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
        }
    }
}