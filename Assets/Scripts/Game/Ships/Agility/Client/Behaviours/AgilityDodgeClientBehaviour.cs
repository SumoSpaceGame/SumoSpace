using Game.Ships.Agility.Common.Abilities;

namespace Game.Ships.Agility.Client.Behaviours
{
    public class AgilityDodgeClientBehaviour : RenderableAbilityBehaviour<AgilityDodgeAbility>
    {
        public override void Execute()
        {
            ((AgilityRenderer)ShipRenderer).DodgeEffect();
        }
    }
}