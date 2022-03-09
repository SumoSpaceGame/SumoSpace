using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities/Agility Abilities", fileName = "Agility Primary Fire", order=1)]
public class AgilityPrimaryFireAbility: ShipAbility {

    public float knockback;

    private Coroutine coroutine;

    public override void ExecuteOnce(ShipManager shipManager, bool isServer) {
        shipManager.GetComponent<AgilityPrimaryFireBehaviour>().ExecuteOnce(shipManager, isServer);
    }

    public override void Execute(ShipManager shipManager, bool isServer) {
        shipManager.GetComponent<AgilityPrimaryFireBehaviour>().Execute(shipManager, isServer);
    }

    public override void Stop(ShipManager shipManager, bool isServer) {
        shipManager.GetComponent<AgilityPrimaryFireBehaviour>().Stop(shipManager, isServer);
    }
    
    public override void AddBehaviour(GameObject gameObject) {
        gameObject.AddComponent<AgilityPrimaryFireBehaviour>();
    }
}
