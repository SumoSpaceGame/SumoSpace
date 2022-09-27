using Game.Common.Gameplay.Abilities;

public abstract class RenderableAbilityBehaviour<T> : AbilityBehaviour<T> where T : ShipAbility {
    private ShipRenderer shipRenderer;
    protected ShipRenderer ShipRenderer {
        get {
            if (!shipRenderer) {
                shipRenderer = shipManager.behaviours[(int)Ability.Slot].gameObject.GetComponent<ShipRenderer>();
            } 
            return shipRenderer;
        }
    }
}