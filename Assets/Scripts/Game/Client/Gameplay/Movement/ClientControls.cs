using Game.Client.Gameplay.Abilities;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using Game.Common.Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Client.Gameplay.Movement
{
    /**
 * Processes client input in-game into movement and abilities
 */
    [RequireComponent(typeof(PlayerInput))]
    public class ClientControls : MonoBehaviour {

        public ShipMovement ShipMovement => shipMovement;

        public ClientShipAbility PrimaryAbility => primaryAbility;

        public Vector2 movementDirection;
        public float movementRotation;
        
        
        [SerializeField] private  ShipMovement shipMovement;
        [SerializeField] private  ClientShipAbility primaryAbility;

        private Vector2 movementVector = Vector2.zero;


        private Vector2 _lookDir;
        
        private Vector2 lookDir
        {
            get
            {
                return _lookDir;
            }

            set
            {
                _lookDir = value;
                movementRotation = (Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg) + RotationAngleOffset;
            }
        }
        
        
        private bool tryDodge;
        private bool startShoot;
        private bool endShoot;
    
        private Camera _camera;

        private const float RotationAngleOffset = -90f;

        private InputLayerNetworkManager inputLayer;
    
        private void Start() {
            _camera = Camera.main;
            inputLayer = MainPersistantInstances.Get<InputLayerNetworkManager>();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update() {
            //movementRotation = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) + RotationAngleOffset;
            //var movementSend = movementVector;
            var sendDodge = tryDodge;



            //inputLayer.SendMovementUpdate(movementSend, rotationSend);
            if (sendDodge) {
                inputLayer.PerformCommand(CommandType.AGILITY_DODGE, new byte[] { });
                tryDodge = false;
            }

            if (startShoot) {
                Debug.Log("PPP");
                inputLayer.PerformCommand(CommandType.START_FIRE, new byte[] { });
                startShoot = false;
            } else if (endShoot) {
                Debug.Log("FF");
                inputLayer.PerformCommand(CommandType.END_FIRE, new byte[] { });
                endShoot = false;
            }
        }

        /**
     * WASD or Left Stick
     */
        public void OnMove(InputAction.CallbackContext ctx) {
            movementVector = ctx.ReadValue<Vector2>();
            movementDirection = movementVector;
        }
    
        /**
     * Mouse position
     */
        public void OnLookRaw(InputAction.CallbackContext ctx) {
            if(ctx.performed)
                lookDir = (Vector2)_camera.ScreenToViewportPoint(ctx.ReadValue<Vector2>()) - new Vector2(0.5f, 0.5f);
        }
    
        /**
     * Right Stick
     */
        public void OnLookNorm(InputAction.CallbackContext ctx) {
            if(ctx.performed)
                lookDir = ctx.ReadValue<Vector2>().normalized;
        }

        /**
     * Shift or A
     */
        public void OnDodge(InputAction.CallbackContext ctx) {
            if (ctx.performed)
                tryDodge = true;
        }
    
        /**
         * LMB or Right Trigger
         */
        public void OnFire(InputAction.CallbackContext ctx) {
            Debug.Log("1");
            if (ctx.started) {
                Debug.Log("2");
                startShoot = true;
            } else if (ctx.canceled) {
                Debug.Log("3");
                endShoot = true;
            }
        }
    }
}
