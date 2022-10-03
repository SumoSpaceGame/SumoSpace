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

    private Vector2 cachedMousePos = Vector2.zero;
    private Vector3 offsetVec = Vector3.forward;
    private Vector3 lookDir;
    private Vector3 lookPos;
    public Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void LateUpdate() {
        offsetVec = Vector3.RotateTowards(
            offsetVec, 
            lookDir * radius, 
            maxRotationSpeed * Mathf.Deg2Rad * 360 * Time.deltaTime, 
            Single.MaxValue
        );
        transform.position = followTarget.position + offsetVec;
        transform.position = MousePosInWorldSpace(cachedMousePos);
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
    public void OnLookRaw(InputAction.CallbackContext ctx) {

        if (ctx.performed)
            /*lookDir = Vec3Util.Vec2ToXZ(
                ((Vector2)_camera.ScreenToViewportPoint(
                    ctx.ReadValue<Vector2>()
                ) - new Vector2(0.5f, 0.5f)).normalized
            );*/
            cachedMousePos = ctx.ReadValue<Vector2>();
    }
    
    public void OnLookNorm(InputAction.CallbackContext ctx) {
        if(ctx.performed)
            lookDir = Vec3Util.Vec2ToXZ(ctx.ReadValue<Vector2>()).normalized;
    }
}