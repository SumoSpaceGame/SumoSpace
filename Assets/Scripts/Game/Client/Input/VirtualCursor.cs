using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualCursor : MonoBehaviour {

    [Tooltip("Radius around the target to spin")]
    public float radius;
    [Tooltip("Transform of the target to follow")]
    public Transform followTarget;
    [Tooltip("Max rotations/sec (for smoothness)")]
    [SerializeField] private float maxRotationSpeed = 1f;

    private Vector3 offsetVec = Vector3.forward;
    private Vector3 lookDir;
    public Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void Update() {
        offsetVec = Vector3.RotateTowards(
            offsetVec, 
            lookDir * radius, 
            maxRotationSpeed * Mathf.Deg2Rad * 360 * Time.deltaTime, 
            Single.MaxValue
        );
        transform.position = followTarget.position + offsetVec;
    }
    
    public void OnLookRaw(InputAction.CallbackContext ctx) { 
        
        if(ctx.performed)
            lookDir = Vec3Util.Vec2ToXZ(
                ((Vector2)_camera.ScreenToViewportPoint(
                    ctx.ReadValue<Vector2>()
                ) - new Vector2(0.5f, 0.5f)).normalized
            );
    }
    
    public void OnLookNorm(InputAction.CallbackContext ctx) {
        if(ctx.performed)
            lookDir = Vec3Util.Vec2ToXZ(ctx.ReadValue<Vector2>()).normalized;
    }
}