using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Processes client input in-game into movement and abilities
 */
[RequireComponent(typeof(PlayerInput))]
public class ClientControls : MonoBehaviour {
    public Ship Ship => ship;

    public ShipMovement ShipMovement => shipMovement;

    public ShipAbility PrimaryAbility => primaryAbility;

    [SerializeField] private Ship ship;
    [SerializeField] private  ShipMovement shipMovement;
    [SerializeField] private  ShipAbility primaryAbility;

    private Vector2 movementVector = Vector2.zero;
    private Vector2 lookDir = Vector2.up;
    
    private Camera camera;

    private const float RotationAngleOffset = -90f;
    
    private void Start() {
        camera = Camera.main;
    }

    private void FixedUpdate() {
        var rotationSend = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + RotationAngleOffset;
        var movementSend = movementVector;
        
        
        

        /*
        // Figure out angular stuff
        ship.rotation = shipMovement.GetRotation(lookDir);
        //atan2(lookDir.y, lookDir.x)

        var desiredVelocity = shipMovement.GetVelocity(movementVector);
        var currentVelocity = ship.rb.velocity;
        var deltaV = desiredVelocity - currentVelocity;
        var force = ship.rb.mass * (deltaV / Time.deltaTime);
        ship.rb.AddForce(force);*/
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
    
    public void OnFire(InputAction.CallbackContext ctx) {
        
    }
}
