using System;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;

namespace UnityTemplateProjects.Game.Common.Gameplay
{
    // If there needs to be rigidbody 2d hard collisions that that needs to be on layers
    public class MatchCollisionFilter: MonoBehaviour
    {
        public int team;
        public bool isGhost;
        public bool pushable;

        private void Start()
        {
            // TODO: Change this to be defined by outside sources
            // For example, 
                // Ship manager
                    // On NetworkStart (After playerID is defined)
                    // Set the team int
                    
                    var shipManager = GetComponent<ShipManager>();
            if (shipManager is null) return;
            var playerData = MainPersistantInstances.Get<GameNetworkManager>().masterSettings
                .GetPlayerData(shipManager.playerMatchID);
            team = playerData.TeamID;
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public bool CanInteract(MatchCollisionFilter other)
        {
            return this.team != other.team;
        }

        public bool CanPush(MatchCollisionFilter other)
        {
            return other.pushable;
        }
    }
}