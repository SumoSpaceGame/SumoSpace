using Game.Common.Gameplay.Ship;
using UnityEngine;

public abstract class AbilityBehaviourComponent : MonoBehaviour
{

    public abstract void Init();
    
    [SerializeField] protected ShipManager shipManager;
    [SerializeField] protected bool executing;
    [SerializeField] protected int oooCounter;
    public abstract void Execute();
    public virtual void QuickExecute() { }

    public virtual void Stop() { }

    public void Init(ShipManager sm)
    {
        shipManager = sm;
        Init();
    }
}