using System;
using System.Collections.Generic;
using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName="Ship Abilities", fileName = "New Loadout",order = 0)]
public class ShipLoadout : ScriptableObject {
    // TODO make a list of ship abilities, maybe make list in manager of behaviours?
    public ShipMovement ShipMovement;

    [Space(2)]
    [Header("Ability list")]
    [Tooltip("First index is Primary Fire ability, second is Primary ability, etc.")]
    [EnumNamedList(typeof(AbilityType))]
    [SerializeField] private List<ShipAbility> abilities = new(Enum.GetValues(typeof(AbilityType)).Length);
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

    public void InitializeBehaviours(ShipManager shipManager) {
        for (int i = 0; i < abilities.Count; i++) {
            abilities[i].AddBehaviour(shipManager, (AbilityType)i);
        }
    }

    /// <summary>
    /// Activates a command whether is stopping, executing, or quick executing
    /// </summary>
    /// <param name="shipManager"></param>
    /// <param name="isServer">Abilities may do different behaviours based on if they are servers or not</param>
    /// <param name="commandType"></param>
    /// <param name="useQuickExecute"></param>
    public void ActivateCommand(ShipManager shipManager, bool isServer, string commandType, bool useQuickExecute = false)
    {
        foreach(var ability in abilities)
        {
            if (ability.ExecuteCommand == commandType)
            {
                if (useQuickExecute)
                {
                    ability.QuickExecute(shipManager, isServer);
                }
                else
                {
                    ability.Execute(shipManager, isServer);
                }
                return;
            }

            if (ability.StopCommand == commandType)
            {
                ability.Stop(shipManager, isServer);
                return;
            }
        }
        
        Debug.Log("Failed to activate command - " + commandType);
    }

    public enum AbilityType {
        PrimaryFire = 0,
        PrimaryAbility = 1,
        SecondaryAbility = 2,
    }
    
    #if UNITY_EDITOR

    public void AddAbility(ShipAbility ability, AbilityType slot)
    {
        abilities[(int)slot] = ability;
    }
    
    public void InitList() 
    {
        while(abilities.Count < Enum.GetValues(typeof(AbilityType)).Length)
        {
            abilities.Add(null);
        }
    }

    #endif
}