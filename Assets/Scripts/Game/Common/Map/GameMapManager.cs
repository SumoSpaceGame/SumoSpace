using System;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Networking.Misc;
using Game.Common.Settings;
using Game.Common.Util;
using UnityEngine;

namespace Game.Common.Map
{
    /// <summary>
    /// Manages all aspects of a map
    /// Spawn points, events that occur, etc
    /// </summary>
    public class GameMapManager : MonoBehaviour, IGameInstance
    {
        
        public MasterSettings masterSettings;

        public MapSettings mapSettings;
        
        private AgentNetworkManager _agentNetworkManager;

        public GameObject gameMapObject;
        public IGameMap gameMap;

        private MatchNetworkTimer timer = null;
        private uint timerID = 0;

        public bool running { private set; get; }
        
        // TODO: Create map event handler
            // Event handler will allow you to timeline things that happen
            // Will communicate on the network too, just supply it with the timeline
            // Can use text announcements (Create a network manager for that too)
            // And activate sub-items that can mean anything for any event
        // TODO: Create a spawn zone, so the player can not leave the spawn.
            // If the player tries to leave, the player will get teleported back to the beginning
            // With a zero velocity too

        private void Awake()
        {
            MainInstances.Add(this);
        }
        
        public void Start()
        {
            
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();

            InitMap();
        }

        public void InitMap()
        {
            
            gameMap = gameMapObject.GetComponent<IGameMap>();

            // TODO: Make this more concerete
            if (gameMap == null)
            {
                // TODO: Add a thing to make sure game match fails
                this.enabled = false;
                Debug.LogError("Failed to get IGameMap component of " + gameMapObject);
                return;
            }
            
            gameMap.Init();
            gameMap.UpdateMap(0);
        }

        /// <summary>
        /// Starts the map based on the timer currently given.
        /// The map manager will handle updating the map elements, and spawning events that occur.
        /// </summary>
        /// <param name="matchTimer"></param>
        public void ActivateMap(uint matchTimer)
        {
            timerID = matchTimer;
            
            // Try to get timer, not guaranteed. Solved in update
            MainPersistantInstances.Get<MatchNetworkTimerManager>().GetTimer(timerID, out timer);
            
            running = true;
        }
        
        
        private void Update()
        {
            //Temp way to solve timer not sync properly.. Needs a better path  
            
            if (!running) return;
            
            if (timer == null && !MainPersistantInstances.Get<MatchNetworkTimerManager>()
                    .GetTimer(masterSettings.matchSettings.timerIDs.mainMatchTimer, out timer))
            {
                return;
            }
            
            if (_agentNetworkManager == null)
            {
                Debug.LogError("Could not find Agent Network Manager");
                this.enabled = false;
                return;
            }
            
            if (masterSettings.network.isServer)
            {
                // TODO: Replace with actual ship damaging system
                var outOfBounds = GetOutOfBoundShips();

                ResetShips(outOfBounds);
            }
            
            gameMap.UpdateMap(Math.Clamp(timer.GetRemainingTimeMinutes()/mapSettings.MatchTimeMinutes, 0, 1));
        }


        private void ResetShips(List<ShipManager> managers)
        {
            // TODO: Reset them to their base
            Vector3 resetPosition = this.transform.position;
            

            foreach (var ship in managers)
            {
                ship.ServerTeleport(resetPosition.toXZ(), false);
            }

        }


        private List<ShipManager> GetOutOfBoundShips()
        {
               
            var ships = _agentNetworkManager._playerShips.GetAll();

            List<ShipManager> outOfBounds = new List<ShipManager>();
            
            foreach(var ship in ships)
            {
                if (IsOutOfBounds(ship.GetWorldPosition(), ship.GetRadius()))
                {
                      outOfBounds.Add(ship);
                }
                
            }

            return outOfBounds;
        }


        private bool IsOutOfBounds(Vector3 position, float radius)
        {
            Vector2 pos = new Vector2(position.x, position.z);
            return !gameMap.WithinMap(pos, radius);
        }

        
        
    }
}