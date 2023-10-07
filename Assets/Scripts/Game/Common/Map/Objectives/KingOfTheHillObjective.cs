using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Common.Map.Objectives
{
    public class KingOfTheHillObjective : MonoBehaviour
    {
        
        public float Radius;
        private Dictionary<int, int> CurrentTeamPoints = new Dictionary<int, int>();

        public float CaptureTime; // How long it takes for capture the point from -1
        public float TakingTimeMultiplier; // Per net ship (net as in 2 reds - 1 blue = 1 red)
        public int PointsPerSecond;
        
        public int CurrentTeamTaken = -1;
        public float CurrentCapture = 0; //  0 = Neutral 
        private Stopwatch stopwatch;

        private MasterSettings masterSettings;

        private void Awake()
        {
            stopwatch = new Stopwatch();
            masterSettings = MainPersistantInstances.Get<GameNetworkManager>().masterSettings;
            Reset();
        }

        public void Reset()
        {
            CurrentTeamPoints = new Dictionary<int, int>();
            CurrentTeamTaken = -1;
            CurrentCapture = CaptureTime;
            CurrentTeamTaken = -3;
        }

        public void Update()
        {
            // Check if ships are in
            float lastCurrentCapture = CurrentCapture;
            // If 
            GetShipsWithin(out var sum, out var team);
            
            if (sum > 0)
            {
                
                if (team != CurrentTeamTaken)
                {
                    CurrentCapture -= Time.deltaTime * TakingTimeMultiplier * sum;
                }
                else
                {
                    CurrentCapture += Time.deltaTime * TakingTimeMultiplier * sum;
                }

                
                // Team has started capturing of the point, move it upwards!
                if (CurrentCapture <= 0)
                {
                    CurrentTeamTaken = -3;
                    CurrentCapture = 0;
                    
                    if (team != -1)
                    {
                        Debug.Log($"Team {team} has started to take the point!");
                        CurrentTeamTaken = team;
                    }
                    else
                    {
                        Debug.Log("Point is now neutral");
                    }
                }

                if (CurrentCapture > CaptureTime) CurrentCapture = CaptureTime;
            } 
            else if (CurrentCapture >= CaptureTime)
            {
                // If it has just been captured
                if (lastCurrentCapture < CaptureTime)
                {
                    CurrentCapture = CaptureTime;

                    if (!CurrentTeamPoints.ContainsKey(CurrentTeamTaken))
                    {
                        CurrentTeamPoints.Add(CurrentTeamTaken, 0);
                    }
                    
                    Debug.Log($"Team {CurrentTeamTaken} has taken the point!");
                    
                    stopwatch.Restart();
                    stopwatch.Start();
                }
                else // TODO: Maybe add a buffer for timing
                {
                    if (stopwatch.IsRunning)
                    {
                        if (stopwatch.ElapsedMilliseconds / 1000.0f >= (1.0f / PointsPerSecond))
                        {
                            CurrentTeamPoints[CurrentTeamTaken] += 1;
                            stopwatch.Restart();
                            stopwatch.Start();
                            Debug.Log($"Team {CurrentTeamTaken} has gained a point!");
                        }
                    }
                }
                

            }
            
            
        }

        /// <summary>
        /// Grabs the ships total in the area
        /// </summary>
        /// <param name="sum">Positive number for ships contesting or capturing</param>
        /// <param name="team">-1 if multiple ships own or no ships found</param>
        public void GetShipsWithin(out int sum, out int team)
        {
            sum = 0; // Total ships that are not on CurrentTeamTaken
            team = -2; // -1 if multi team

            var ships = masterSettings.playerShips.GetAll();

            Vector2 curPos = new Vector2(this.transform.position.x, this.transform.position.z);

            float sqrRadius = this.Radius * this.Radius;

            foreach (var ship in ships)
            {
                var shipPos = ship.Get2DPosition();
                float sqrMag = (curPos - shipPos).sqrMagnitude;
                
                if (sqrMag < sqrRadius)
                {
                    var data = masterSettings.GetPlayerData(ship.playerMatchID);
                    
                    // If no one has the point
                    if (data.TeamID != CurrentTeamTaken || CurrentTeamTaken == -3)
                    {
                        sum += 1;
                        
                        if (team == -2) // Team not assigned for this check
                        {
                            team = data.TeamID;
                        }
                        else if (team != data.TeamID) // Multi team capture
                        {
                            team = -1;
                        }
                    }
                    else
                    {
                        sum -= 1;
                    }

                }
            }
            
            sum = Mathf.Abs(sum);
            
            if (team == -2) team = -1;
            
            Debug.Log(sum + " " +  team);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(this.transform.position, Radius);
        }
        private void OnDrawGizmosSelected()
        {
            OnDrawGizmos();
        }
    }
}