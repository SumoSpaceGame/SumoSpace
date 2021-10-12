using UnityEngine;

[CreateAssetMenu(menuName = "Ship Movement")]
public class ShipMovement : ScriptableObject {
    [Header("Movement Settings")]
    [Tooltip("Max speed of the ship in units/sec")]
    [SerializeField] private float maxSpeed = 8f; // Max speed of the ship in units/sec
    [Tooltip("Max rotation speed of the ship in rotations/sec")]
    [Range(0f, 2f)]
    [SerializeField] private float maxTheta = 1f; // Max rotation in rotations/sec
    [Tooltip("Bigger Turn Radius = less agile")]
    [SerializeField] private float turnRadius = 4f; // Self explanatory
    [Tooltip("How long it takes the ship to reach full speed from full stop or full stop from full speed")]
    [SerializeField] private float accelTime = 0.5f; // The time the full acceleration ramp takes
    [SerializeField] private AnimationCurve accelCurve; // Curve that represents the acceleration of the ship
    [Tooltip("Modifier of how fast the ship moves going backwards. Always <= 1")]
    [Range(0f, 1f)]
    [SerializeField] private float backwardsSpeedFactor = 0.6f; // The multiplication factor that sets the speed of the ship when going backwards

    private Vector2 prevDir = Vector2.zero; // Cached last movement
    private float prevRot; // Cached last look
    
    private bool stopped = true; // If the ship is not moving
    private float x; // The progress along the acceleration curve
    
    private const float AngleOffset = -90; // Offset so that vector2.up is forward (by default 0 is to the right)

    private bool locked;

    public Vector2 GetVelocity(Vector2 movementDir) {

        if (locked) return prevDir;
        
        if (movementDir != Vector2.zero) { 
            stopped = false;
            x += Time.deltaTime;
            x = Mathf.Clamp(x , 0, accelTime);
            // Interpolate between 
            var movDeltaAngle = Mathf.Clamp(Vector2.SignedAngle(prevDir, movementDir) * Mathf.Deg2Rad, -turnRadius * Time.deltaTime, turnRadius * Time.deltaTime);
            var curMovAngle = Mathf.Atan2(prevDir.y, prevDir.x);
            var newMovAngle = curMovAngle + movDeltaAngle;
            prevDir = stopped ? movementDir : new Vector2(Mathf.Cos(newMovAngle), Mathf.Sin(newMovAngle));
        } else {
            x -= Time.deltaTime;
            x = Mathf.Clamp(x, 0, accelTime);
            if (x < float.Epsilon) {
                stopped = true;
            }
        }
        // Avoid divide by zero errors when editing the value in the inspector
        accelTime = accelTime == 0 ? 1 : accelTime;
        
        var rawVel = prevDir * (accelCurve.Evaluate(Mathf.Clamp(x / accelTime, 0f, 1f)) * maxSpeed);
        var correctionFactor = 
            Mathf.Lerp(
                backwardsSpeedFactor, 
                1.0f, 
                (
                    Vector2.Dot(
                        rawVel.normalized, 
                        new Vector2(Mathf.Cos(prevRot * Mathf.Deg2Rad), Mathf.Sin(prevRot * Mathf.Deg2Rad))
                    ) + 1
                ) / 2
            );
        var targetVel = rawVel * correctionFactor;
        return targetVel;
    }
    
    public float GetRotation(Vector2 lookDir) {
        
        if (locked) return prevRot;
        
        // Get the mouse angle, do some interpolation with the current facing direction, rotate
        var targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + AngleOffset;
        var newAngle = Mathf.MoveTowardsAngle(prevRot, targetAngle,
            maxTheta * 360 * Time.deltaTime);
        prevRot = newAngle;
        return newAngle;
    }

    public void SetLocked(bool l) {
        locked = l;
    }
}
