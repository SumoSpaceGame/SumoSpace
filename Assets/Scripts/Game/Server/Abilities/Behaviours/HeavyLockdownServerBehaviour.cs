using Game.Common.Gameplay.Ship;
using System.Collections;
using UnityEngine;

public class HeavyLockdownServerBehaviour : AbilityBehaviour<HeavyLockdownAbility>
{
    private Coroutine coroutine;
    private float maxForce = 0;
    private HeavyPrimaryFireAbility heavyFire = null;

    public override void Execute()
    {
        if (heavyFire is null)
        {
            heavyFire = shipManager.shipLoadout.PrimaryFire as HeavyPrimaryFireAbility;
            if (heavyFire is null)
            {
                Debug.LogError("No Heavy Primary Fire Ability found.");
                return;
            }
        }
        if (heavyFire.IsDisabled)
            return;
        if (maxForce == 0)
            maxForce = shipManager.shipLoadout.ShipMovement.maxForce;
        if (!executing)
        {
            coroutine = shipManager.StartCoroutine(ServerSideStart());
            executing = true;
        }
        else
        {
            coroutine = shipManager.StartCoroutine(ServerSideStop());
            executing = false;
        }
    }

    // Winds up and activates the lockdown.
    private IEnumerator ServerSideStart()
    {
        shipManager.shipController.SetSpeedMultiplier(0);
        heavyFire.IsDisabled = true;
        yield return new WaitForSeconds(Ability.WindUpTime);
        heavyFire.IsDisabled = false;
        heavyFire.KnockbackMultiplier = Ability.KnockbackMultiplier;
        shipManager.shipLoadout.ShipMovement.maxForce *= Ability.ForceMultiplier;
        yield return null;
    }

    // Winds down and deactivates the lockdown.
    private IEnumerator ServerSideStop()
    {
        heavyFire.KnockbackMultiplier = 1;
        shipManager.shipLoadout.ShipMovement.maxForce = maxForce;
        heavyFire.IsDisabled = true;
        yield return new WaitForSeconds(Ability.WindDownTime);
        heavyFire.IsDisabled = false;
        shipManager.shipController.SetSpeedMultiplier(1);
        yield return null;
    }
}