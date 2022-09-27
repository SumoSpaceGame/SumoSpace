using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Map
{
    public class TempCircleArena : MonoBehaviour
    {
        public MasterSettings masterSettings;

        public float Radius;
        
        private AgentNetworkManager _agentNetworkManager;
        
        public void Start()
        {
            
            if (!masterSettings.network.isServer)
            {
                this.enabled = false;
            }

            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
        }


        private void Update()
        {

            if (_agentNetworkManager == null)
            {
                Debug.LogError("Could not find Agent Netowrk Manager");
                this.enabled = false;
                return;
            }

            var outOfBounds = GetOutOfBoundShips();

            ResetShips(outOfBounds);
        }


        private void ResetShips(List<ShipManager> managers)
        {

            Vector3 resetPosition = this.transform.position;


            foreach (var ship in managers)
            {
                resetPosition.z = ship.transform.position.z;
                ship.transform.position = resetPosition;
            }

        }


        private List<ShipManager> GetOutOfBoundShips()
        {
               
            var ships = _agentNetworkManager._playerShips.GetAll();

            List<ShipManager> outOfBounds = new List<ShipManager>();
            
            foreach(var ship in ships)
            {
                if (IsOutOfBounds(ship.transform.position))
                {
                    outOfBounds.Add(ship);
                }
                
            }

            return outOfBounds;
        }


        private bool IsOutOfBounds(Vector3 position)
        {
            var sqrDistance = (position - this.transform.position).sqrMagnitude;

            return sqrDistance > Mathf.Pow(Radius, 2);
        }
    }
}
