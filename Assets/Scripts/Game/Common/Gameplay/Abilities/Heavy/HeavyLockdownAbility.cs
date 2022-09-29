using Game.Common.Gameplay.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Heavy Abilities/Heavy Lockdown", fileName = "Heavy Lockdown", order = 2)]
public class HeavyLockdownAbility : ShipAbility
{
    [SerializeField] private float length;
    /// <summary>
    /// The length, in seconds, of the lockdown.
    /// </summary>
    public float Length => length;

    [SerializeField] private float knockbackMultiplier;
    public float KnockbackMultiplier => knockbackMultiplier;

    [SerializeField] private float resistanceMultiplier;
    public float ResistanceMultiplier => resistanceMultiplier;
}
