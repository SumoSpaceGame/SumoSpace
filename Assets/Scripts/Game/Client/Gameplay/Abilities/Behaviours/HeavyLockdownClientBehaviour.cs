using System.Collections;
using UnityEngine;

public class HeavyLockdownClientBehaviour : RenderableAbilityBehaviour<HeavyLockdownAbility>
{
    public override void Execute()
    {
        
    }

    public override void QuickExecute()
    {
        Debug.Log("Starting Lockdown on client.");
        if (++oooCounter == 1)
        {
            executing = true;
        }
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(Ability.Time);
        executing = false;
    }

}