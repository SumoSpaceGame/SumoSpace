using Game.Client.Gameplay.Abilities;
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

        [SerializeField] private  ShipMovement shipMovement;
        [SerializeField] private  ClientShipAbility primaryAbility;

        private Vector2 movementVector = Vector2.zero;
        private Vector2 lookDir = Vector2.up;
    
        private Camera _camera;

        private const float RotationAngleOffset = -90f;

        private InputLayerNetworkManager inputLayer;
    
        private void Start() {
            _camera = Camera.main;
            inputLayer = MainPersistantInstances.Get<InputLayerNetworkManager>();
        }

        private void FixedUpdate() {
            var rotationSend = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) + RotationAngleOffset;
            var movementSend = movementVector;
            
            
        
            inputLayer.SendMovementUpdate(movementSend, rotationSend);
        }

        /**
     * WASD or Left Stick
     */
        public void OnMove(InputAction.CallbackContext ctx) {
            movementVector = ctx.ReadValue<Vector2>();
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
        
        }
    
        public void OnFire(InputAction.CallbackContext ctx) {
        
        }
    }
}
