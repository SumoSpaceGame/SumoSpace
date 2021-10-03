using System;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Class that moves the player around in 3D (XZ plane)
 * This should not be used as instead we want to modify the simulation which runs on the XY plane
 * This class is just being kept as reference, just in case
 */
[RequireComponent(typeof(PlayerInput))]
public class ClientMovement3D : MonoBehaviour {
    //[SerializeField] private ClientInput input;
    [SerializeField] private Ship3D ship;
    //[SerializeField] private VirtualCursor directionIndicator;
    
    [Header("Movement Settings")]
    [Tooltip("Max speed of the ship in units/sec")]
    [SerializeField] private float maxSpeed = 8f; // Max speed of the ship in units/sec
    [Tooltip("Max rotation speed of the ship in rotations/sec")]
    [Range(0f, 2f)]
    [SerializeField] private float maxTheta = 1f; // Max rotation in rotations/sec
    [Tooltip("Bigger Turn Radius = less agile")]
    [SerializeField] private float turnRadius = 4f; // Self explanatory
    // TODO Remove maxAccel?
    [Tooltip("How quickly you can change movement speed once already moving. Only affects controller because KB is normalized")]
    [SerializeField] private float maxAccel = 5f; // Acceleration when changing direction 
    [Tooltip("How long it takes the ship to reach full speed from full stop or full stop from full speed")]
    [SerializeField] private float accelTime = 0.5f; // The time the full acceleration ramp takes
    [SerializeField] private AnimationCurve accelCurve; // Curve that represents the acceleration of the ship
    /*
     Probably unecessary
    [SerializeField] private bool separateDecelerationCurve = false;
    [SerializeField] private float decelTime; // The time the full deceleration ramp takes
    [SerializeField] private AnimationCurve decelCurve; // Curve that represents the deceleration of the ship
    */
    [Tooltip("Modifier of how fast the ship moves going backwards. Always < 1")]
    [Range(0f, 1f)]
    [SerializeField] private float backwardsSpeedFactor = 0.6f; // The multiplication factor that sets the speed of the ship when going backwards

    private Vector3 prevDir = Vector3.zero; // For interpolation purposes
    
    private Vector3 lookDir = Vector3.forward; // Set by input
    private Vector3 movementDir = Vector3.zero; // Set by input
    
    private bool stopped = true; // If the ship is not moving
    private float x; // The progress along the acceleration curve

    private Camera camera;

    private void Start() {
        camera = Camera.main;
    }

    void Update() {
        
        // Alternate idea for mouse look: Have an invisible plane, find the point where the mouse is on the plane, look at that point
        
        // Get the mouse angle, do some interpolation with the current facing direction, rotate
        var shipQuat = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDir), maxTheta * 360 * Time.deltaTime);
        ship.rotation = shipQuat.eulerAngles;
        
        if (movementDir != Vector3.zero) {
            stopped = false;
            x += Time.deltaTime;
            x = Mathf.Clamp(x , 0, accelTime);
            // Interpolate between 
            prevDir = Vector3.RotateTowards(
                prevDir, 
                movementDir, 
                // If stopped we do not care about the last direction we moved in (no reason to smoothly interpolate)
                stopped ? Single.MaxValue : turnRadius * Time.deltaTime, 
                maxAccel * Time.deltaTime);
        } else {
            x -= Time.deltaTime;
            x = Mathf.Clamp(x, 0, accelTime);
            if (x < Single.Epsilon) {
                stopped = true;
            }
        }
        // Avoid divide by zero errors when editing the value in the inspector
        accelTime = accelTime == 0 ? 1 : accelTime;
        
        var rawVel = prevDir * (accelCurve.Evaluate(Mathf.Clamp(x / accelTime, 0f, 1f)) * maxSpeed);
        var correctionFactor = Mathf.Lerp(backwardsSpeedFactor, 1.0f, (Vector3.Dot(rawVel.normalized, lookDir.normalized) + 1) / 2);
        var targetVel = rawVel * correctionFactor;
        ship.velocity = targetVel;
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        movementDir = Vec3Util.Vec2ToXZ(ctx.ReadValue<Vector2>());
    }
    
    public void OnLookRaw(InputAction.CallbackContext ctx) {
        lookDir = Vec3Util.Vec2ToXZ(
            ((Vector2)camera.ScreenToViewportPoint(
                ctx.ReadValue<Vector2>()
                ) - new Vector2(0.5f, 0.5f)).normalized
            );
    }
    
    public void OnLookNorm(InputAction.CallbackContext ctx) {
        if(ctx.performed)
            lookDir = Vec3Util.Vec2ToXZ(ctx.ReadValue<Vector2>()).normalized;
    }
}