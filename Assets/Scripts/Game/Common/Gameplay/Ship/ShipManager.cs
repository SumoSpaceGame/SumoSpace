using System;
using System.Collections.Generic;
using FishNet;
using Game.Client.Gameplay.Movement;
using Game.Common.Instances;
using Game.Common.Map;
using Game.Common.Networking;
using Game.Common.Registry;
using Game.Common.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityTemplateProjects.Game.Common.Gameplay;

namespace Game.Common.Gameplay.Ship
{
    public class ShipManager : MonoBehaviour {
        public float Rotation => transform.eulerAngles.z;
        public Vector3 Position => transform.position;
        public bool invulnerable;
        public Animator animator;
        [FormerlySerializedAs("rigidbody2D")] public Rigidbody2D _rigidbody2D;
        public ShipController shipController;
        public ClientControls clientControls;

        public ShipLoadout shipLoadout;
        
        public SimulationObject simulationObject;
        public AgentMovementNetworkManager networkMovement;
        public GameObject virtualCursorPrefab;
        public VirtualCursor virtualCursor;

        public MatchCollisionFilter matchCollisionFilter;

        [EnumNamedList(typeof(ShipLoadout.AbilityType))]
        public List<AbilityBehaviourComponent> behaviours = new List<AbilityBehaviourComponent>(Enum.GetValues(typeof(ShipLoadout.AbilityType)).Length);
        
        [Space(2)]
        [Header("Defined on creation")]
        public bool isServer = false;

        // True, if this manages manages the client's player ship (per instance)
        public bool isPlayer;
    

        public PlayerID playerMatchID;
        private CircleCollider2D circleCollider2D;

        private void Awake()
        {
            circleCollider2D = this.GetComponent<CircleCollider2D>();
        }

        public void SetLayer(LayerMask layer)
        {
            this.gameObject.layer = layer;
            SetLayerChildren(gameObject, layer);
        }

        private void SetLayerChildren(GameObject obj, LayerMask layer)
        {
            foreach (GameObject child in this.transform)
            {
                child.layer = layer;
                SetLayerChildren(child, layer);
            }
        }
    
    
        private void Start() {
            simulationObject.Create();
            shipLoadout.InitializeBehaviours(this);
            if (isPlayer) {
                UnityEngine.Camera.main.GetComponent<CameraFollow>().followTarget = simulationObject.representative.transform;
                GetComponent<PlayerInput>().enabled = true;
                
                virtualCursor = Instantiate(virtualCursorPrefab).GetComponent<VirtualCursor>();
                virtualCursor.followTarget = simulationObject.representative.transform;
            } else {
                clientControls.enabled = false;
                GetComponent<PlayerInput>().enabled = false;
            }
        }

        public void EnableColliders(bool enable = true) {
            var colliders = new Collider2D[3];
            _rigidbody2D.GetAttachedColliders(colliders);
            foreach (var collider in colliders) {
                if(collider != null) {
                    collider.isTrigger = !enable;
                }
            }
        }
        public void OnLookRaw(InputAction.CallbackContext ctx) { 
            virtualCursor.OnLookRaw(ctx);
        }
    
        public void OnLookNorm(InputAction.CallbackContext ctx) {
            virtualCursor.OnLookNorm(ctx);
        }

        public float GetRadius()
        {
            return circleCollider2D.radius;
        }

        public Vector3 GetWorldPosition()
        {
            return simulationObject.representative.transform.position;
        }

        public Vector2 Get2DPosition()
        {
            return new Vector2(this.transform.position.x, this.transform.position.y);
        }

        public void Kill()
        {
            var shipSpawner = MainInstances.Get<ShipSpawnManager>();
            var master = MainPersistantInstances.Get<GameNetworkManager>().masterSettings;

            if (InstanceFinder.IsServer)
            {            
                this.transform.position = shipSpawner.GetRespawnPoint(master.matchSettings.ClientTeam, master.matchSettings.ClientTeamPosition).toSimulationPlane();
            }
            else
            {
                // Do the kill    
            }
        }
        
        


        /// <summary>
        /// Teleports the ship at the the server by the server
        ///
        /// TODO: Create more handlers for maybe effects and such?
        /// </summary>
        /// <param name="position"></param>
        /// <param name="keepInertia"></param>
        public void ServerTeleport(Vector2 position, bool keepInertia)
        {
            if (!isServer)
            {
                Debug.LogError("Failed to teleport, can not teleport on client!");
            }
            
            if (!keepInertia)
            {
                shipController.ResetInertia();
            }

            this.transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}
