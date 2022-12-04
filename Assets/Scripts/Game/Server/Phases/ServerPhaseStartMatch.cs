using System;
using System.Collections.Generic;
using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Map;
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
            _matchNetworkTimerManager = MainPersistantInstances.Get<MatchNetworkTimerManager>();

        }
        
        public void PhaseStart()
        {
            _matchNetworkTimerManager = MainPersistantInstances.Get<MatchNetworkTimerManager>();
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
            timer = null;
            _matchNetworkTimerManager.GetTimer(_gamePhaseNetworkManager.gameMatchSettings.timerIDs.mainMatchTimer,
                out timer);
            /*
            Debug.Log("TEST = " + _gamePhaseNetworkManager.gameMatchSettings.timerIDs.mainMatchTimer);
            
            //Send to clients to sync timer
            
            Debug.Log("Test 2 " + _matchNetworkTimerManager.GetTimer(_gamePhaseNetworkManager.gameMatchSettings.timerIDs.mainMatchTimer,
                out timer));
            Debug.Log("Test 2 " + timer);*/
            
            // TODO: Restructure how this works. GameMapManager should already be in the scene.
            
            timer.StartTimer((long) (_gamePhaseNetworkManager.masterSettings.matchSettings.SelectedMapItem.mapSettings.MatchTimeMinutes * 60 * 1000));
            timer.StopEvent += OnTimerFinished;

            playerCounter = new PlayerCounter(_gamePhaseNetworkManager.masterSettings.GetPlayerCount());

            List<byte> byteArr = new List<byte>();
            byteArr.Add(0);
            byteArr.AddRange(BitConverter.GetBytes(timer.ID));
            _gamePhaseNetworkManager.SendPhaseUpdate(_gamePhaseNetworkManager.CurrentPhase, byteArr.ToArray());
            
            // Now anyone who leaves will be subjected to the reconnect method, if it works hehe
            _gamePhaseNetworkManager.masterSettings.matchSettings.ServerRestartOnLeave = false;
            
            
        }

        public void PhaseUpdate()
        {
            if (MainInstances.HasType(typeof(GameMapManager)))
            {
                var map = MainInstances.Get<GameMapManager>();

                if (!map.running)
                {
                    map.ActivateMap(timer.ID);
                }
            }
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
                    Debug.Log("Spawning ships");
                    _gamePhaseNetworkManager.masterSettings.DebugLogPlayerStatic();

                    var spawnpositions = MainInstances.Get<ShipSpawnManager>();
                    
                    foreach (PlayerID player in playerCounter.GetPlayers())
                    {
                        var staticData = _gamePhaseNetworkManager.masterSettings.playerStaticDataRegistry.Get(player);
                        
                        _agentNetworkManager.SpawnShip(player, spawnpositions.GetSpawnPoint(staticData.TeamID, staticData.TeamPosition));
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