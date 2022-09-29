using System.Collections;
using UnityEngine;

public class HeavyLockdownServerBehaviour : AbilityBehaviour<HeavyLockdownAbility>
{
    private Coroutine coroutine;

    public override void Execute()
    {
        coroutine ??= shipManager.StartCoroutine(ServerSide());
        executing = true;
    }

    // TODO shoot from the correct points (not out front)
    private IEnumerator ServerSide()
    {
        var counter = 0f;
        var timer = 0f;
        bool lockdownActive = true;
        float maxSpeed = 0;
        HeavyPrimaryFireAbility primaryFire = shipManager.shipLoadout.PrimaryFire as HeavyPrimaryFireAbility;
        ShipMovement movement = shipManager.shipLoadout.ShipMovement;
        while (counter >= 0)
        {
            counter += Time.deltaTime / Ability.Cooldown;
            timer = executing ? timer + Time.deltaTime : 0f;
            if (counter > 1 && executing)
            {
                if (timer <= Ability.Length)
                {
                    if (primaryFire is not null && !lockdownActive)
                    {
                        Debug.Log("Currently in Lockdown state.");
                        primaryFire.KnockbackMultiplier = Ability.KnockbackMultiplier;
                        movement.maxForce *= Ability.ResistanceMultiplier;
                        maxSpeed = movement.maxSpeed;
                        movement.maxSpeed = 0;
                        lockdownActive = true;
                    }
                    timer += Time.deltaTime;
                }
            }
            if (!executing || timer > Ability.Length)
            {
                if (lockdownActive)
                {
                    movement.maxForce /= Ability.ResistanceMultiplier;
                    lockdownActive = false;
                }
                primaryFire.KnockbackMultiplier = 1;
                movement.maxSpeed = maxSpeed == 0 ? movement.maxSpeed : maxSpeed;
                executing = false;
                timer = 0;
            }
            yield return null;
        }
    }
}