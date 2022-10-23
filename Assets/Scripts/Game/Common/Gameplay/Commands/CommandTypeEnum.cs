namespace Game.Common.Gameplay.Commands
{
    /// <summary>
    /// All abilities that are registered in the system should have an enum here.
    /// </summary>
    public enum CommandType
    {
        AGILITY_DODGE,
        AGILITY_PRIMARY_FIRE_START,
        AGILITY_PRIMARY_FIRE_END,
        HEAVY_PRIMARY_FIRE_START,
        HEAVY_PRIMARY_FIRE_END,
        HEAVY_LOCKDOWN,
    }
}