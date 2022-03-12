public abstract class AbilityBehaviour<T> : AbilityBehaviourComponent where T : ShipAbility {
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
