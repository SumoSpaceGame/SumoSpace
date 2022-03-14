using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
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
        }
        
        public void PhaseStart()
        {
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
            timer = _matchNetworkTimerManager.CreateTimer();
            
            //Send to clients to sync timer
            
            timer.StartTimer(30 * 1000);
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

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            if (data.Length == 1)
            {
                playerCounter.Register(_gamePhaseNetworkManager.masterSettings.playerIDRegistry.Get(info.SendingPlayer.NetworkId));

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