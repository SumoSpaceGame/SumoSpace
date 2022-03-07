﻿using System;
using System.Collections.Generic;
using System.Data;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities", fileName = "New Loadout",order = 0)]
public class ShipLoadout : ScriptableObject {
    //TODO make a list of ship abilities, maybe make list in manager of behaviours?
    public ShipMovement ShipMovement;

    [Space(2)]
    [Header("Ability list")]
    [Tooltip("First index is Primary Fire ability, second is Primary ability, etc.")]
    [EnumNamedList(typeof(AbilityType))]
    [SerializeField] private List<ShipAbility> abilities;
    public ShipAbility PrimaryFire => abilities[(int)AbilityType.PrimaryFire];
    public ShipAbility PrimaryAbility => abilities[(int)AbilityType.PrimaryAbility];
    public ShipAbility SecondaryAbility => abilities[(int)AbilityType.SecondaryAbility];

    public T GetAbility<T>() where T : ShipAbility {
        foreach (var shipAbility in abilities) {
            if (shipAbility is T ability) {
                return ability;
            }
        }
        return null;
    }

    public void InitializeBehaviours(ShipManager manager) {
        var go = manager.gameObject;
        foreach (var shipAbility in abilities) {
            shipAbility.AddBehaviour(go);
        }
    }
    
    public enum AbilityType {
        PrimaryFire,
        PrimaryAbility,
        SecondaryAbility,
    }
}