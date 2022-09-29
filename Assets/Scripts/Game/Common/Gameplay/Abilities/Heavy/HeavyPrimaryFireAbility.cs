using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Primary Fire", fileName = "Heavy Primary Fire", order = 1)]
public class HeavyPrimaryFireAbility : ShipAbility {

    public float KnockbackMultiplier = 1;

    [SerializeField] private float minKnockback;
    public float MinKnockback => minKnockback * KnockbackMultiplier;

    [SerializeField] private float maxKnockback;
    public float MaxKnockback => maxKnockback * KnockbackMultiplier;

    [SerializeField] private float rampTime;
    public float RampTime => rampTime;
    
    [SerializeField] private AnimationCurve knockbackCurve;
    public AnimationCurve KnockbackCurve => knockbackCurve;

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
        float curveValue = knockbackCurve.Evaluate(time / rampTime);   //  Between 0 and 1.
        return curveValue * (MaxKnockback - MinKnockback) + MinKnockback;
    }
}
