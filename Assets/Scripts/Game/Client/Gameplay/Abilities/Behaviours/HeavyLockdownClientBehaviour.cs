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
    }

    public override void Stop()
    {
        if (--oooCounter == 0)
        {
            executing = false;
        }
    }
}