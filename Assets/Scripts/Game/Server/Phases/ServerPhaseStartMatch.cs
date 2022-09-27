using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Networking.Misc;
using Game.Common.Networking.Utility;
using Game.Common.Phases;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseStartMatch : IGamePhase
    {

        private GamePhaseNetworkManager _gamePhaseNetworkManager;
        private MatchNetworkTimerManager _matchNetworkTimerManager;
        private AgentNetworkManager _agentNetworkManager;

        private MatchNetworkTimer timer;

        public PlayerCounter playerCounter;
        
        public ServerPhaseStartMatch(GamePhaseNetworkManager gamePhaseNetworkManager, MatchNetworkTimerManager matchNetworkTimerManager)
        {
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
            _matchNetworkTimerManager = matchNetworkTimerManager;

            // Now anyone who leaves will be subjected to the reconnect method, if it works hehe
            _gamePhaseNetworkManager.masterSettings.matchSettings.ServerRestartOnLeave = false;
        }
        
        public void PhaseStart()
        {
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
            timer = _matchNetworkTimerManager.CreateTimer();
            
            //Send to clients to sync timer
            
            timer.StartTimer(30 * 60 * 1000);
            timer.StopEvent += OnTimerFinished;

            playerCounter = new PlayerCounter(_gamePhaseNetworkManager.masterSettings.GetPlayerCount());
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

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            
            PlayerID playerID;

            if (!_gamePhaseNetworkManager.masterSettings.playerIDRegistry.TryGetByNetworkID(conn.ClientId, out playerID))
            {
                Debug.LogWarning("Non-registered player tried to push start match! " + conn.GetAddress());
                return;
            }
            
            if (data.Length == 1)
            {
                playerCounter.Register(playerID);

                if (playerCounter.IsFull())
                {
                    
                    foreach (PlayerID player in playerCounter.GetPlayers())
                    {
                        _agentNetworkManager.SpawnShip(player);
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid packet recieved for server phase start match");
            }
        }
    }
}