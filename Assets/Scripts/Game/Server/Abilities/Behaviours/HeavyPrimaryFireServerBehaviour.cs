using System.Collections;
using UnityEngine;

public class HeavyPrimaryFireServerBehaviour : AbilityBehaviour<HeavyPrimaryFireAbility> {
    private Coroutine coroutine;
    
    public override void Execute() {
        coroutine ??= shipManager.StartCoroutine(ServerSide());
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void Stop() {
        if (--oooCounter == 0) {
            executing = false;
        }
    }
    
    // TODO shoot from the correct points (not out front)
    private IEnumerator ServerSide() {
        var counter = 0f;
        var timer = 0f;
        while (counter >= 0) {
            counter += Time.deltaTime / Ability.Cooldown;
            timer = executing ? timer + Time.deltaTime : 0f;
            if (counter > 1 && executing) {
                var t = shipManager.transform;
                var hit = Physics2D.Raycast(t.position + t.up * 2, t.up);
                if (hit.rigidbody) {
                    hit.rigidbody.AddForceAtPosition(t.up * Ability.CurrentKnockback(timer), hit.point, ForceMode2D.Force);
                }
                counter = 0;
            }
            yield return null;
        }
    }
}