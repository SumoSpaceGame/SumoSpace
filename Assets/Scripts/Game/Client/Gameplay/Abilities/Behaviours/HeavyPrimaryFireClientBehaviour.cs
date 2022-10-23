using System.Collections;
using UnityEngine;

public class HeavyPrimaryFireClientBehaviour : RenderableAbilityBehaviour<HeavyPrimaryFireAbility> {
    private Coroutine coroutine;
    private bool _shouldBeamStart = false;

    public override void Execute() {

    }

    public override void QuickExecute() {
        if (!Ability.IsDisabled)
        {
            ShipRenderer.StartBeam();
            coroutine = shipManager.StartCoroutine(ClientSide());
        }
        else
            _shouldBeamStart = true;
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void Stop() {
        if (coroutine != null) shipManager.StopCoroutine(coroutine);
        ShipRenderer.EndBeam();
        _shouldBeamStart = false;
        if (--oooCounter == 0) {
            executing = false;
        }
    }

    private IEnumerator ClientSide() {
        while (true) {
            if (Ability.IsDisabled)
            {
                executing = false;
                ShipRenderer.EndBeam();
                _shouldBeamStart = true;
                shipManager.StopCoroutine(coroutine);
            }
            else
                ShipRenderer.Beam();
            yield return null;
        }
    }

    // If the fire ability is reenabled while executing, start the beam.
    private void Update()
    {
        if (!Ability.IsDisabled && _shouldBeamStart)
        {
            _shouldBeamStart = false;
            QuickExecute();
        }
    }
}