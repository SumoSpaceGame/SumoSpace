using System;
using System.Collections;
using System.Collections.Generic;
using Game.Client.Gameplay.Movement;
using Game.Common.Networking;
using Game.Common.Registry;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

        [EnumNamedList(typeof(ShipLoadout.AbilityType))]
        public List<AbilityBehaviourComponent> behaviours = new List<AbilityBehaviourComponent>(Enum.GetValues(typeof(ShipLoadout.AbilityType)).Length);
        
        [Space(2)]
        [Header("Defined on creation")]
        public bool isServer = false;

        // True, if this manages manages the client's player ship (per instance)
        public bool isPlayer;
    

        public PlayerID playerMatchID;

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
            return this.GetComponent<CircleCollider2D>().radius;
        }

    }
}
