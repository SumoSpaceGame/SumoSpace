using Game.Common.Gameplay.Ship;
using System.Collections;
using UnityEngine;

public class HeavyLockdownServerBehaviour : AbilityBehaviour<HeavyLockdownAbility>
{
    private Coroutine coroutine;

    public override void Execute()
    {
        if (!executing)
            coroutine ??= shipManager.StartCoroutine(ServerSide());
        executing = true;
    }

    // Activates the lockdown, waits for the correct amount of time, and then deactivates the lockdown.
    private IEnumerator ServerSide()
    {
        Debug.Log("Starting lockdown.");
        HeavyPrimaryFireAbility hpfAbility = shipManager.shipLoadout.PrimaryAbility as HeavyPrimaryFireAbility;
        if (hpfAbility == null)
        {
            Debug.LogError("No Heavy Primary Fire Ability found.");
        }
        else
        {
            hpfAbility.KnockbackMultiplier = Ability.KnockbackMultiplier;
            float maxForce = shipManager.shipLoadout.ShipMovement.maxForce;
            shipManager.shipLoadout.ShipMovement.maxForce *= Ability.ForceMultiplier;
            shipManager.shipController.SetSpeedMultiplier(0);
            yield return new WaitForSeconds(Ability.Time);
            hpfAbility.KnockbackMultiplier = 1;
            shipManager.shipLoadout.ShipMovement.maxForce = maxForce;
            shipManager.shipController.SetSpeedMultiplier(1);
            Debug.Log("Ending lockdown.");
        }
        executing = false;
        yield return null;
    }
}