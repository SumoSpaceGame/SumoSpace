using Game.Client.Gameplay.Movement;
using Game.Common.Networking;
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
        public SimulationObject simulationObject;
        public AgentMovementNetworkManager networkMovement;
    
        [Space(2)]
        [Header("Defined on creation")]
        public bool isServer = false;

        public bool isPlayer;
    

        public ushort playerMatchID;
    
    
    
        private void Start()
        {
        
            simulationObject.Create();
            if (isPlayer)
            {
                UnityEngine.Camera.main.GetComponent<CameraFollow>().followTarget = this.simulationObject.representative.transform;
                GetComponent<PlayerInput>().enabled = true;
            }
            else
            {
                clientControls.enabled = false;
                GetComponent<PlayerInput>().enabled = false;
            }
        }
    }
}
