using Game.Common.Gameplay.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Primary Fire", fileName = "Heavy Primary Fire", order = 1)]
public class HeavyPrimaryFireAbility : ShipAbility {

    [SerializeField] private float minKnockback;
    public float MinKnockback => minKnockback;

    [SerializeField] private float maxKnockback;
    public float MaxKnockback => maxKnockback;

    [SerializeField] private float rampTime;
    public float RampTime => rampTime;
    
    [SerializeField] private AnimationCurve knockbackCurve;
    public AnimationCurve KnockbackCurve => knockbackCurve;
}
