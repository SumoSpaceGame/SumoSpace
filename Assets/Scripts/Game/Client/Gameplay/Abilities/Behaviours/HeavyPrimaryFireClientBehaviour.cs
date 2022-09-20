using System.Collections;
using UnityEngine;

public class HeavyPrimaryFireClientBehaviour : RenderableAbilityBehaviour<HeavyPrimaryFireAbility> {
    private Coroutine coroutine;

    public override void Execute() {
        
    }

    public override void QuickExecute() {
        ShipRenderer.StartBeam();
        coroutine ??= shipManager.StartCoroutine(ClientSide());
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void Stop() {
        if(coroutine != null) shipManager.StopCoroutine(coroutine);
        ShipRenderer.EndBeam();
        if (--oooCounter == 0) {
            executing = false;
        }
    }

    private IEnumerator ClientSide() {
        while (true) {
            ShipRenderer.Beam();
            yield return null;
        }
    }
}