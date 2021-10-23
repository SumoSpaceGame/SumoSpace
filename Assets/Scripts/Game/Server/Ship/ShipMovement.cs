using UnityEngine;

[CreateAssetMenu(menuName = "Ship Movement")]
public class ShipMovement : ScriptableObject {
    [Header("Movement Settings")]
    [Tooltip("Max speed of the ship in units/sec")]
    public float maxSpeed = 8f; // Max speed of the ship in units/sec
    [Tooltip("Max rotation speed of the ship in rotations/sec")]
    [Range(0f, 2f)]
    public float maxTheta = 1f; // Max rotation in rotations/sec
    [Tooltip("Bigger Turn Radius = less agile")]
    public float turnRadius = 4f; // Self explanatory
    [Delayed]
    [Tooltip("How long it takes the ship to reach full speed from full stop or full stop from full speed")]
    public float accelTime = 0.5f; // The time the full acceleration ramp takes
    public AnimationCurve accelCurve; // Curve that represents the acceleration of the ship
    [Tooltip("Modifier of how fast the ship moves going backwards. Always <= 1")]
    [Range(0f, 1f)]
    public float backwardsSpeedFactor = 0.6f; // The multiplication factor that sets the speed of the ship when going backwards
}
