using System.Collections;
using Game.Common.Gameplay.Ship;
using UnityEngine;

// TODO split client and server behaviors
public class AgilityPrimaryFireBehaviour : AbilityBehaviour<AgilityPrimaryFireAbility> {
    private Coroutine coroutine;
    private bool eatNextShot;
    
    public override void Execute(ShipManager shipManager, bool isServer) {
        coroutine ??= shipManager.StartCoroutine(isServer ? ServerSide(shipManager) : ClientSide(shipManager));
        if (++oooCounter == 1) {
            executing = true;
        }
    }

    public override void ExecuteOnce(ShipManager shipManager, bool isServer) {
        if (isServer) return;
        shipManager.simulationObject.representative.GetComponent<ShipRenderer>().Shoot();
        eatNextShot = true;
    }

    public override void Stop(ShipManager shipManager, bool isServer) {
        if (--oooCounter == 0) {
            executing = false;
        }
    }
    
    private IEnumerator ServerSide(ShipManager shipManager) {
        var counter = 0f;
        while (counter >= 0) {
            counter += Time.deltaTime / ability.Cooldown;
            if (counter > 1 && executing) {
                var t = shipManager.transform;
                var hit = Physics2D.Raycast(t.position + t.up * 2, t.up);
                if (hit.rigidbody) {
                    hit.rigidbody.AddForceAtPosition(t.up * ability.knockback, hit.point, ForceMode2D.Impulse);
                }
                counter = 0;
            }
            yield return null;
        }
    }

    private IEnumerator ClientSide(ShipManager shipManager) {
        var counter = 0f;
        while (counter >= 0) {
            counter += Time.deltaTime / ability.Cooldown;
            if (counter > 1 && executing) {
                if (!eatNextShot) {
                    shipManager.simulationObject.representative.GetComponent<ShipRenderer>().Shoot();
                } else {
                    eatNextShot = false;
                }
                counter = 0;
            }
            yield return null;
        }
    }
}
