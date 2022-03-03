using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities/Agility Abilities", fileName = "Agility Primary Fire", order=1)]
public class AgilityPrimaryFireAbility: ShipAbilityToggle {

    [SerializeField] private float rateOfFire; // shots / second
    private Coroutine coroutine;
    public override void Execute(ShipManager shipManager, bool isServer) {
        coroutine = shipManager.StartCoroutine(isServer ? ServerSide(shipManager) : ClientSide(shipManager));
    }

    public override void Stop(ShipManager shipManager, bool isServer) {
        shipManager.StopCoroutine(coroutine);
    }

    IEnumerator ServerSide(ShipManager shipManager) {
        var counter = 0f;
        while (counter >= 0) {
            counter += rateOfFire * Time.deltaTime;
            if (counter > 1) {
                var t = shipManager.transform;
                var hit = Physics2D.Raycast(t.position + t.up * 2, t.up);
                if (hit.rigidbody) {
                    hit.rigidbody.AddForceAtPosition(t.up * 10, hit.point, ForceMode2D.Impulse);
                }
                counter = 0;
            }
            yield return null;
        }
    }

    IEnumerator ClientSide(ShipManager shipManager) {
        var counter = 0f;
        while (counter >= 0) {
            counter += rateOfFire * Time.deltaTime;
            if (counter > 1) {
                shipManager.simulationObject.representative.GetComponent<ShipRenderer>().Shoot();
                counter = 0;
            }
            yield return null;
        }
    }
}
