using Game.Common.Gameplay.Abilities;

public abstract class AbilityBehaviour<T> : AbilityBehaviourComponent where T : ShipAbility {
    public override void Init()
    {
        Ability.Init();
    }
    
    private T ability;
    
    protected T Ability {
        get {
            if (ability == null) {
                ability = shipManager.shipLoadout.GetAbility<T>();
            }

            return ability;
        }
    }
}
