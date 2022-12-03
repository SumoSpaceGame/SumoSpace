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

        public List<MapTrackerBase> mapTrackers = new List<MapTrackerBase>(); // A map tracker is used to determine what layer on the map it is on
        
        public GameMapBoundaryList gameBoundaries = new GameMapBoundaryList();
        
        private AgentNetworkManager _agentNetworkManager;

        public GameObject gameMapObject;
        public IGameMap gameMap;

        private MatchNetworkTimer timer = null;
        private uint timerID = 0;

        public float DeadZoneDistance; // Distance from an edge where the map tracker gets set to dead

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
            
            UpdateTrackers();
            
            gameMap.UpdateMap(Math.Clamp(timer.GetRemainingTimeMinutes()/mapSettings.MatchTimeMinutes, 0, 1));
        }
        

        private void UpdateTrackers()
        {
            List<MapTrackerBase> trackers = new List<MapTrackerBase>();
            
            float dist = DeadZoneDistance * DeadZoneDistance;
            
            foreach (var tracker in mapTrackers)
            {
                float radius = tracker.GetRadius();
                Vector2 trackerPos = tracker.GetPosition();
                bool within = false;
                
                if (radius <= 0)
                {
                    within = gameMap.WithinMap(trackerPos);
                }
                else
                {
                    within = gameMap.WithinMap(trackerPos, radius);
                }

                if (within)
                {
                    tracker.currentLayer = MapTrackerBase.CurrentLayer.SAFE;
                }
                else
                {
                    if (gameMap.SqrDistanceFromEdge(tracker.GetPosition()) <= dist)
                    {
                        tracker.currentLayer = MapTrackerBase.CurrentLayer.WARNING;
                    }
                    else
                    {
                        tracker.currentLayer = MapTrackerBase.CurrentLayer.DEAD;
                    }
                }
                
                
                // Compare against game boundaries now
                // Boundary comparison is based on the priority of the elements
                // If the ship is in within an boundary, it can only get the greatest layer within that priority level
                // Lower priorities will be ignored
                var sortedBoundaries = gameBoundaries.GetBoundaries();
                
                int lastPriority = sortedBoundaries[sortedBoundaries.Length - 1].Priority;

                bool foundBoundary = false;
                
                for (int i = sortedBoundaries.Length - 1; i >= 0; i--)
                {
                    
                    var boundary = sortedBoundaries[i];
                    bool withinBoundary = false;

                    if (foundBoundary && lastPriority != boundary.Priority)
                    {
                        break;
                    }
                    
                    if (boundary.Priority <= 0)
                    {
                        if (within)
                        {
                            break;
                        }
                    }
                    
                    if (radius <= 0)
                    {
                        withinBoundary = boundary.IsWithin(trackerPos);
                    }
                    else
                    {
                        withinBoundary = boundary.IsWithin(trackerPos, radius);
                    }

                    if (withinBoundary)
                    {
                        foundBoundary = true;

                        tracker.SetGreatest(radius <= 0
                            ? boundary.GetLayer(trackerPos)
                            : boundary.GetLayer(trackerPos, radius));
                    }


                    
                    lastPriority = boundary.Priority;
                }

            }
            
            
        }

        
        
    }
}