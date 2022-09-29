using System.Collections;
using UnityEngine;

public class HeavyLockdownClientBehaviour : RenderableAbilityBehaviour<HeavyLockdownAbility>
{
    private Coroutine coroutine;

    public override void Execute()
    {

    }

    public override void QuickExecute()
    {
        var routine = shipManager.StartCoroutine(ClientSide());
        coroutine ??= routine;
    }

    private IEnumerator ClientSide()
    {
        float timer = 0;
        while (timer < Ability.Length)
        {
            Debug.Log("Currently in Lockdown state.");
            timer += Time.deltaTime;
            yield return null;
        }
    }
}