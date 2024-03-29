using System.Collections;
using Game.Common.Gameplay.Ship;
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
    public class ClientControls : MonoBehaviour 
    {
        private ShipManager ShipManager => shipManager;

        public Vector2 movementDirection;
        public float movementRotation;
        
        [SerializeField] private ShipManager shipManager;
        private ShipLoadout _shipLoadout;

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
                movementRotation = (Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg) + ROTATION_ANGLE_OFFSET;
            }
        }
        
        public Coroutine fireCoroutine;
        
        
        private bool sendPrimaryAbility, sendSecondaryAbility;
        private bool shooting;
    
        private Camera _camera;

        private const float ROTATION_ANGLE_OFFSET = -90f;

        private InputLayerNetworkManager inputLayer; 
    
        private void Start() {
            // TODO: Don't use Camera.main
            _camera = Camera.main;
            inputLayer = MainPersistantInstances.Get<InputLayerNetworkManager>();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            sendPrimaryAbility = false;
            sendSecondaryAbility = false;
            _shipLoadout = shipManager.shipLoadout;
        }

        private void OnDestroy()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update() {
            //movementRotation = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) + RotationAngleOffset;
            //var movementSend = movementVector;

            if (inputLayer == null)
            {
                inputLayer = MainPersistantInstances.Get<InputLayerNetworkManager>();
                return;
            }

            //inputLayer.SendMovementUpdate(movementSend, rotationSend);
            if (sendPrimaryAbility) 
            {
                sendPrimaryAbility = false;
                
                if (_shipLoadout.PrimaryAbility == null)
                {
                    Debug.LogError("Failed to execute primary ability. None set");
                }
                else
                {
                    //inputLayer.PerformCommand(CommandType.AGILITY_DODGE, new byte[] { });
                    inputLayer.PerformCommand(_shipLoadout.PrimaryAbility.ExecuteCommand, new byte[] {});
                }
                
            }
            
            if (sendSecondaryAbility) 
            {
                sendSecondaryAbility = false;
                
                if (_shipLoadout.SecondaryAbility == null)
                {
                    Debug.LogError("Failed to execute secondary ability. None set");
                }
                else
                {
                    //inputLayer.PerformCommand(CommandType.AGILITY_DODGE, new byte[] { });
                    inputLayer.PerformCommand(_shipLoadout.SecondaryAbility.ExecuteCommand, new byte[] {});
                }
            }
        }

        // TODO Render shot here
        IEnumerator Shoot() {
            while(true){
                //inputLayer.PerformCommand(CommandType.AGILITY_DODGE, new byte[] { });
                //inputLayer.PerformCommand(ShipLoadout.PrimaryFire.Command, new byte[] {});
                
                //yield return new WaitForSeconds(ShipMovement.timeBetweenShots);
                Debug.Log("Shooting");
                yield return new WaitForSeconds(_shipLoadout.PrimaryFire.Cooldown);
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
            if (ctx.performed) {
                //lookDir = (Vector2)_camera.ScreenToViewportPoint(ctx.ReadValue<Vector2>()) - new Vector2(0.5f, 0.5f);
                var v3 = MousePosInWorldSpace(ctx.ReadValue<Vector2>()) - MousePosInWorldSpace(_camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)));
                lookDir = new Vector2(v3.x, v3.z);
            }
        }
        
        private Vector3 MousePosInWorldSpace(Vector2 mousePos) {
            var plane = new Plane(Vector3.up, 0);
            float dist;
            var ray = _camera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
            Vector3 worldPos = Vector3.zero;
            if (plane.Raycast(ray, out dist)) {
                worldPos = ray.GetPoint(dist);
            }

            return worldPos;
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
        public void OnPrimaryAbility(InputAction.CallbackContext ctx) {
            if (ctx.performed)
                sendPrimaryAbility = true;
        }
        
        /**
     * Shift or A
     */
        public void OnSecondaryAbility(InputAction.CallbackContext ctx) {
            if (ctx.performed)
                sendSecondaryAbility = true;
        }
        
        /**
         * LMB or Right Trigger
         */
        public void OnFire(InputAction.CallbackContext ctx) {
            if (ctx.started) {
                //fireCoroutine = StartCoroutine(Shoot());
                inputLayer.PerformCommand(_shipLoadout.PrimaryFire.ExecuteCommand, new byte[] {});
            } else if (ctx.canceled) {
                //StopCoroutine(fireCoroutine);
                inputLayer.PerformCommand(_shipLoadout.PrimaryFire.StopCommand, new byte[] {});
            }
        }
    }
}
