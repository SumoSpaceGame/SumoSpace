using Game.Common.Gameplay.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Primary Fire", fileName = "Heavy Primary Fire", order = 1)]
public class HeavyPrimaryFireAbility : ShipAbility {

    [SerializeField] private float minKnockback;
    /// <summary>
    /// Minimum knockback after applying multipliers.
    /// </summary>
    public float MinKnockback => minKnockback * KnockbackMultiplier;

    [SerializeField] private float maxKnockback;
    /// <summary>
    /// Maximum knockback after applying multipliers.
    /// </summary>
    public float MaxKnockback => maxKnockback * KnockbackMultiplier;

    [SerializeField] private float rampTime;
    public float RampTime => rampTime;
    
    [SerializeField] private AnimationCurve knockbackCurve;
    public AnimationCurve KnockbackCurve => knockbackCurve;

    public float KnockbackMultiplier = 1f;

    /// <summary>
    /// Returns the current amount of knockback that should be used.
    /// </summary>
    /// <remarks>
    /// Knockback scales linearly until maximum is reached.
    /// </remarks>
    /// <param name="time"> The amount of time the ability has been active.</param>
    /// <returns></returns>
    public float CurrentKnockback(float time)
    {
        float charge = KnockbackCurve.Evaluate(time); // Between 0 and 1.
        return Mathf.Lerp(MinKnockback, MaxKnockback, charge);
    }
}
