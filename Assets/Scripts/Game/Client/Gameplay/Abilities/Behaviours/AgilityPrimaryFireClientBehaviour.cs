using System.Collections;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class AgilityPrimaryFireClientBehaviour : RenderableAbilityBehaviour<AgilityPrimaryFireAbility> {
    private Coroutine coroutine;
    private bool eatNextShot;

    public override void Execute() {
        coroutine ??= shipManager.StartCoroutine(ClientSide());
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void QuickExecute() {
        ShipRenderer.Shoot();
        eatNextShot = true;
    }

    public override void Stop() {
        if (--oooCounter == 0) {
            executing = false;
        }
    }

    private IEnumerator ClientSide() {
        var counter = 0f;
        while (true) {
            counter += Time.deltaTime / Ability.Cooldown;
            if (counter > 1 && executing) {
                if (!eatNextShot) {
                    ShipRenderer.Shoot();
                } else {
                    eatNextShot = false;
                }
                counter = 0;
            }
            yield return null;
        }
    }
}
