using System;
using System.Collections.Generic;
using System.Diagnostics;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        [Serializable]
        public struct KingOfTheHillSettings
        {
            public int CaptureRate;   // How main points per ship will start capturing this
            public int FreeRate;      // How hard it is to free this point
            public int CaptureRequirement; // How many points are required to "capture this item

            public int CaptureIncome; // How much points to gather while capturing
            public int CircleRadius;
            
            [Space(4)]
            [Header("Tick Rates in Seconds")]
            public float CaptureTickRate;
            public float FreeTickRate;
            public float PointTickRate;
        }

        public Transform circleCenter;

        [SerializeField]
        private KingOfTheHillState state = KingOfTheHillState.IDLE;

        private bool taken = false;
        private int focalTeam = -1; // The team that is the focal point of the state (capturing, captured, freeing, etc)
        private int CurrentCapturePoints = 0;
        
        public KingOfTheHillSettings settings = new KingOfTheHillSettings();

        public Stopwatch capturePointTimer = new Stopwatch();
        public Stopwatch freePointTimer = new Stopwatch();
        public Stopwatch scorePointTimer = new Stopwatch();
        
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

            if (scorePointTimer.ElapsedMilliseconds >= settings.PointTickRate * 1000)
            {
                Debug.Log("Team " + focalTeam + " gained "  + settings.CaptureIncome +" points");
                // TODO: Add proper score counting here
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(this.transform.position, this.settings.CircleRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.GetRadiusScale());
        }

        private void UpdateIdle(ShipsWithinInfo withinInfo)
        {
            Debug.Log("Idle");
            if (state != KingOfTheHillState.IDLE)
            {
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
                    CurrentCapturePoints += settings.CaptureRate;
                    withinInfo = GetShipsWithin();
                    UpdateCapturing(withinInfo); // Start capturing
                }
                            
            }
        }
        
        private void UpdateCapturing(ShipsWithinInfo withinInfo)
        {
            Debug.Log("Capturing");
            if (state != KingOfTheHillState.CAPTURING)
            {
                state = KingOfTheHillState.CAPTURING;
                capturePointTimer.Restart();
            }

            if (capturePointTimer.ElapsedMilliseconds >= settings.CaptureTickRate * 1000)
            {
                capturePointTimer.Restart();
                this.CurrentCapturePoints += settings.CaptureRate;
            }
            
            
            switch (withinInfo.status)
            {
                case PointStatus.LOSING:
                    capturePointTimer.Stop();
                    UpdateFreeing(withinInfo);
                    break;
                case PointStatus.CONTESTED:
                    capturePointTimer.Stop();
                    UpdateContested(withinInfo);
                    break;
                case PointStatus.WINNING:
                    if (IsCaptureRequirementMet())
                    {
                        capturePointTimer.Stop();
                        UpdateIdle(withinInfo);
                    }
                    break;
                case PointStatus.IDLE:
                    capturePointTimer.Stop();
                    UpdateIdle(withinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void UpdateFreeing(ShipsWithinInfo withinInfo)
        {
            Debug.Log("Freeing");
            if (state != KingOfTheHillState.FREEING)
            {
                freePointTimer.Restart();
                state = KingOfTheHillState.FREEING;
            }

            if (freePointTimer.ElapsedMilliseconds >= settings.FreeTickRate * 1000)
            {
                freePointTimer.Restart();
                CurrentCapturePoints -= settings.FreeRate;
            }
            
            switch (withinInfo.status)
            {
                case PointStatus.LOSING:
                    // Keep on trucking along
                    if (CurrentCapturePoints <= 0)
                    {
                        freePointTimer.Stop();
                        CurrentCapturePoints = 0;
                        RemoveFocalTeam();
                        UpdateIdle(withinInfo);
                    }
                    break;
                case PointStatus.CONTESTED:
                    freePointTimer.Stop();
                    UpdateContested(withinInfo);
                    break;
                case PointStatus.WINNING:
                    freePointTimer.Stop();
                    UpdateCapturing(withinInfo);
                    break;
                case PointStatus.IDLE:
                    freePointTimer.Stop();
                    UpdateIdle(withinInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

               
            
        }

        private void UpdateContested(ShipsWithinInfo withinInfo)
        {
            Debug.Log("Contested");
            if (state != KingOfTheHillState.CONTESTED)
            {
                state = KingOfTheHillState.CONTESTED;
            }

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
            
            float sqrRadius = settings.CircleRadius * settings.CircleRadius;
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
                
                Debug.Log("Ships found");
            }
            
            Debug.Log(status);
            

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


        private void SetFocalTeam(int focalTeam)
        {
            Debug.Log("Team " + focalTeam + " has gained the point");
            scorePointTimer.Restart();
            taken = true;
            this.focalTeam = focalTeam;
        }

        private void RemoveFocalTeam()
        {
            Debug.Log("Team " + focalTeam + " has lost the point");
            scorePointTimer.Reset();
            taken = false;
            focalTeam = -1;
        }



        public bool IsTaken()
        {
            return taken;
        }

        public int GetFocalTeam()
        {
            return focalTeam;
        }

        public float GetRadiusScale()
        {
            float progress = (float)CurrentCapturePoints / (float)settings.CaptureRequirement;

            return progress * settings.CircleRadius;
        }
    }
}