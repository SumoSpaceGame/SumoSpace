using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Map
{
    public class GameMapManager : MonoBehaviour
    {
        
        public MasterSettings masterSettings;

        private AgentNetworkManager _agentNetworkManager;

        public GameObject gameMapObject;
        public IGameMap gameMap;
        
        public void Start()
        {
            
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();

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
        
        
        
        private void Update()
        {

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

        }


        private void ResetShips(List<ShipManager> managers)
        {
            // TODO: Reset them to their base
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