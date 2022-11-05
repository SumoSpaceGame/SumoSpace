using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class HeavyPrimaryFireClientBehaviour : RenderableAbilityBehaviour<HeavyPrimaryFireAbility> {
    private Coroutine coroutine;
    private bool _shouldBeamStart = false;
    private VisualEffect _fireEffect;

    public override void Execute() {

    }

    public override void QuickExecute() {
        _fireEffect ??= shipManager.gameObject.GetComponent<VisualEffect>();
        if (!Ability.IsDisabled)
        {
            //ShipRenderer.StartBeam();
            _fireEffect.Play();
            coroutine = shipManager.StartCoroutine(ClientSide());
        }
        else
            _shouldBeamStart = true;
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void Stop() {
        _fireEffect ??= shipManager.gameObject.GetComponent<VisualEffect>();
        if (coroutine != null) shipManager.StopCoroutine(coroutine);
        //ShipRenderer.EndBeam();
        _fireEffect.Stop();
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
                //ShipRenderer.EndBeam();
                _fireEffect.Stop();
                _shouldBeamStart = true;
                shipManager.StopCoroutine(coroutine);
            }
            else
            {
                //ShipRenderer.Beam();
                _fireEffect.Play();
            }
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