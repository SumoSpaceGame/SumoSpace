using System;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public abstract class AbilityBehaviour<T> : MonoBehaviour where T : ShipAbility {
    [SerializeField] protected T ability;
    [SerializeField] protected bool executing;
    [SerializeField] protected int oooCounter;
    
    private void Start() {
        ability = GetComponent<ShipManager>().shipLoadout.GetAbility<T>();
    }

    public abstract void Execute(ShipManager manager, bool isServer);
    public virtual void ExecuteOnce(ShipManager manager, bool isServer) { }

    public virtual void Stop(ShipManager shipManager, bool isServer) { }
}
