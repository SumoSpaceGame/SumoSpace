using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Processes client input in-game into movement and abilities
 */
[RequireComponent(typeof(PlayerInput))]
public class ClientControls : MonoBehaviour {
    
    [SerializeField] private Ship ship;
    public ShipMovement shipMovement;
    public ShipAbility primaryAbility;

    private Vector2 movementDir = Vector2.zero;
    private Vector2 lookDir = Vector2.up;
    
    private Camera camera;
    
    private void Start() {
        camera = Camera.main;
    }

    private void FixedUpdate() {
        // Figure out angular stuff
        ship.rotation = shipMovement.GetRotation(lookDir);

        var desiredVelocity = shipMovement.GetVelocity(movementDir);
        var currentVelocity = ship.rb.velocity;
        var deltaV = desiredVelocity - currentVelocity;
        var force = ship.rb.mass * (deltaV / Time.deltaTime);
        ship.rb.AddForce(force);
    }

    /**
     * WASD or Left Stick
     */
    public void OnMove(InputAction.CallbackContext ctx) {
        movementDir = ctx.ReadValue<Vector2>();
    }
    
    /**
     * Mouse position
     */
    public void OnLookRaw(InputAction.CallbackContext ctx) {
        if(ctx.performed)
            lookDir = (Vector2)camera.ScreenToViewportPoint(ctx.ReadValue<Vector2>()) - new Vector2(0.5f, 0.5f);
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
}
