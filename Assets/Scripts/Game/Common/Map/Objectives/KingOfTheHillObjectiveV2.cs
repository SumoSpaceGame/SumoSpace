using System;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Map.Objectives
{
    public enum KingOfTheHillState
    {
        IDLE = 0,       // No one is on the point, so nothing is happening
        CAPTURING = 1,  // The point is being taken about another team
        FREEING = 2,    // The point is being freed from being taken
        CONTESTED = 3,  // The point is being contested
    }
    
    public class KingOfTheHillObjectiveV2 : MonoBehaviour
    {

        private MasterSettings masterSettings;

        public struct KingOfTheHillSettings
        {
            public int CaptureRate;   // How main points per ship will start capturing this
            public int FreeRate;      // How hard it is to free this point
            public int CaptureRequirement; // How many points are required to "capture this item

            public int CaptureIncome; // How much points to gather while capturing
            public int CircleRadius;
            public float TickRate;
        }

        public Transform circleCenter;

        [SerializeField]
        private KingOfTheHillState state = KingOfTheHillState.IDLE;

        private bool taken = false;
        private int focalTeam = -1; // The team that is the focal point of the state (capturing, captured, freeing, etc)
        private int CurrentCapturePoints = 0;
        
        public KingOfTheHillSettings settings = new KingOfTheHillSettings();
        
        private void Awake()
        {
            masterSettings = MainPersistantInstances.Get<GameNetworkManager>().masterSettings;
        }

        public void Update()
        {
            var shipsWithinInfo = GetShipsWithin();

            switch (state)
            {
                case KingOfTheHillState.IDLE:
                    UpdateIdle(shipsWithinInfo);
                    break;
                case KingOfTheHillState.CAPTURING:
                    UpdateCapturing(shipsWithinInfo);
                    break;
                case KingOfTheHillState.FREEING:
                    UpdateFreeing(shipsWithinInfo);
                    break;
                case KingOfTheHillState.CONTESTED:
                    UpdateContested(shipsWithinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateIdle(ShipsWithinInfo withinInfo)
        {
            if (state != KingOfTheHillState.IDLE)
            {
                // TODO: Stop all point calculations and timers
                state = KingOfTheHillState.IDLE;
            }

            if (withinInfo.totalShipsWithin <= 0) return;
            
            if (taken)
            {
                           
                switch (withinInfo.status)
                {
                    case PointStatus.LOSING:
                        UpdateFreeing(withinInfo);
                        break;
                    case PointStatus.CONTESTED:
                        UpdateContested(withinInfo);
                        break;
                    case PointStatus.WINNING:
                        if (!IsCaptureRequirementMet()) UpdateCapturing(withinInfo);
                        break;
                    case PointStatus.IDLE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                                
                            
            }
            else
            {
                // TODO: Discuss the design of the king of the hill
                // Should focalTeam be decided as soon as the point if free for the taking
                // Should focalTeam be decided as soon as the point is taken to allow for sniping play
                // ???
                            
                // For now its as soon as the point is taken

                if (!withinInfo.freePointContested)
                {
                    // TODO: Start score point incrementor once this team reaches full capture
                    // Or maybe not, maybe it should start right away
                    SetFocalTeam(withinInfo.teamMajority);
                    UpdateCapturing(withinInfo); // Start capturing
                }
                            
            }
        }
        
        private void UpdateCapturing(ShipsWithinInfo withinInfo)
        {
            if (state != KingOfTheHillState.CAPTURING)
            {
                // TODO: Start giving capturing points
                state = KingOfTheHillState.CAPTURING;
            }
            
            
            
            switch (withinInfo.status)
            {
                case PointStatus.LOSING:
                    UpdateFreeing(withinInfo);
                    break;
                case PointStatus.CONTESTED:
                    UpdateContested(withinInfo);
                    break;
                case PointStatus.WINNING:
                    if (IsCaptureRequirementMet())
                    {
                        UpdateIdle(withinInfo);
                    }
                    break;
                case PointStatus.IDLE:
                    UpdateIdle(withinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void UpdateFreeing(ShipsWithinInfo withinInfo)
        {
            if (state != KingOfTheHillState.FREEING)
            {
                // TODO: Start removing the points
                state = KingOfTheHillState.FREEING;
            }
            
            
            switch (withinInfo.status)
            {
                case PointStatus.LOSING:
                    // Keep on trucking along
                    if (CurrentCapturePoints <= 0)
                    {
                        CurrentCapturePoints = 0;
                        RemoveFocalTeam();
                        UpdateIdle(withinInfo);
                    }
                    break;
                case PointStatus.CONTESTED:
                    UpdateContested(withinInfo);
                    break;
                case PointStatus.WINNING:
                    UpdateCapturing(withinInfo);
                    break;
                case PointStatus.IDLE:
                    UpdateIdle(withinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

               
            
        }

        private void UpdateContested(ShipsWithinInfo withinInfo)
        {
            if (state != KingOfTheHillState.CONTESTED)
            {
                // TODO: Stop all point calculations and timers
                state = KingOfTheHillState.CONTESTED;
            }
            
            // TODO: Start contested graphics
            
            switch (withinInfo.status)
            {
                case PointStatus.LOSING:
                    UpdateFreeing(withinInfo);
                    break;
                case PointStatus.CONTESTED:
                    // Do nothing until point status changes
                    break;
                case PointStatus.WINNING:
                    UpdateCapturing(withinInfo);
                    break;
                case PointStatus.IDLE:
                    UpdateIdle(withinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }


        private enum PointStatus
        {
            LOSING = 0,
            CONTESTED,
            WINNING,
            IDLE
        }
        private struct ShipsWithinInfo
        {
            public Dictionary<int, int> perTeamShipsCount;
            public int totalShipsWithin;
            public PointStatus status;
            public int teamMajority;
            public bool freePointContested;
        }
        
        private ShipsWithinInfo GetShipsWithin()
        {
            var ships = masterSettings.playerShips.GetAll();

            float sqrRadius = settings.CircleRadius;
            Vector2 currentPosition = new Vector2(circleCenter.position.x, circleCenter.position.z);

            Dictionary<int, int> shipsWithin = new Dictionary<int, int>();

            int totalShips = 0;
            
            foreach (var ship in ships)
            {
                var shipPos = ship.Get2DPosition();

                float sqrMag = (currentPosition - shipPos).sqrMagnitude;

                // Within the circle
                if (sqrMag < sqrRadius)
                {
                    var data = masterSettings.GetPlayerData(ship.playerMatchID);

                    if (!shipsWithin.ContainsKey(data.TeamID))
                    {
                        shipsWithin.Add(data.TeamID, 1);
                    }

                    else
                    {
                        shipsWithin[data.TeamID] = shipsWithin[data.TeamID]++;
                    }

                    totalShips++;
                }
            }

            PointStatus status = PointStatus.IDLE;
            int winningLimit = (int)Math.Ceiling((double)totalShips / 2.0);
            int focalTeamShipCount = shipsWithin.ContainsKey(focalTeam) ? shipsWithin[focalTeam] : 0;

            if (totalShips > 0)
            {
                if (!shipsWithin.ContainsKey(focalTeam) ||
                    focalTeamShipCount < winningLimit)
                {
                    status = PointStatus.LOSING;
                }
                else if(shipsWithin.ContainsKey(focalTeam) && totalShips % 2 == 0 && focalTeamShipCount > winningLimit)
                {
                    status = PointStatus.WINNING;
                }
                else if(shipsWithin.ContainsKey(focalTeam) && totalShips % 2 == 1 && focalTeamShipCount >= winningLimit)
                {
                    status = PointStatus.WINNING;
                }
                else
                {
                    status = PointStatus.CONTESTED;
                }
            }

            bool contested = false;
            int biggestTeam = -1;
            int teamCount = -1;
            
            foreach(var within in shipsWithin)
            {
                if (within.Value > teamCount)
                {
                    teamCount = within.Value;
                    biggestTeam = within.Key;
                    contested = false;
                }
                else if (within.Value == teamCount)
                {
                    // If the are two biggest teams, this is contested
                    contested = true;
                }
                
            }

            if (contested)
            {
                biggestTeam = -1;
            }
            
            
            return new ShipsWithinInfo
            {
                perTeamShipsCount = shipsWithin,
                totalShipsWithin = totalShips,
                status = status,
                freePointContested = contested, 
                teamMajority = biggestTeam,
            };

        }


        private bool IsCaptureRequirementMet()
        {
            if (CurrentCapturePoints > settings.CaptureRequirement) CurrentCapturePoints = settings.CaptureRequirement;
            return CurrentCapturePoints >= settings.CaptureRequirement;
        }


        public void SetFocalTeam(int focalTeam)
        {
            taken = true;
            focalTeam = focalTeam;
        }

        public void RemoveFocalTeam()
        {
            taken = false;
            focalTeam = -1;
        }
    }
}