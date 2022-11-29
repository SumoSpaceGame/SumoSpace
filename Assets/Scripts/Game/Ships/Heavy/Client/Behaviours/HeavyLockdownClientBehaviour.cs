using System.Collections;
using Game.Ships.Heavy.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Heavy.Client.Behaviours
{
    public class HeavyLockdownClientBehaviour : RenderableAbilityBehaviour<HeavyLockdownAbility>
    {
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
            if (!executing)
            {
                shipManager.StartCoroutine(ClientSideStart());
                executing = true;
            }
            else
            {
                shipManager.StartCoroutine(ClientSideStop());
                executing = false;
            }
        }

        // Winds up and activates the lockdown.
        private IEnumerator ClientSideStart()
        {
            heavyFire.IsDisabled = true;
            yield return new WaitForSeconds(Ability.WindUpTime);
            heavyFire.IsDisabled = false;
            yield return null;
        }

        // Winds down and deactivates the lockdown.
        private IEnumerator ClientSideStop()
        {
            heavyFire.IsDisabled = true;
            yield return new WaitForSeconds(Ability.WindDownTime);
            heavyFire.IsDisabled = false;
            yield return null;
        }

        /*public override void QuickExecute()
    {
        if (++oooCounter == 1)
        {
            executing = true;
        }
    }

    public override void Stop()
    {
        if (--oooCounter == 0)
        {
            executing = false;
        }
    }*/
    }
}