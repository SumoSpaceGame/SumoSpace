namespace Game.Common.Gameplay.Ship
{
    /// <summary>
    /// Stats for a ship in the game, this will define everything that makes them spawnable
    /// </summary>
    public struct ShipCreationData
    {
        // TODO: Fill this out with the required items for a ship
        public int shipType;


        public static ShipCreationData Create(int shipTypeTEMP)
        {
            ShipCreationData data = new ShipCreationData();
            data.shipType = shipTypeTEMP;

            return data;
        }
    }
}