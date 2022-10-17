using Game.Common.Gameplay.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Lockdown", fileName = "Heavy Lockdown", order = 2)]
public class HeavyLockdownAbility : ShipAbility
{
    [Tooltip("Multiplier applied to the Heavy Primary Fire during Lockdown")]
    [SerializeField] private float _knockbackMultiplier;
    [Tooltip("Multiplier applied to the ship's max force.")]
    [SerializeField] private float _forceMultiplier;
    [Tooltip("Amount of time the lockdown is active, in seconds.")]
    [SerializeField] private float _time;

    /// <summary>
    /// Multiplier applied to the Heavy Primary Fire.
    /// </summary>
    public float KnockbackMultiplier => _knockbackMultiplier;

    /// <summary>
    /// Multiplier applied to the ship's max force.
    /// </summary>
    public float ForceMultiplier => _forceMultiplier;
    /// <summary>
    /// Amount of time the lockdown is active, in seconds.
    /// </summary>
    public float Time => _time;
}
