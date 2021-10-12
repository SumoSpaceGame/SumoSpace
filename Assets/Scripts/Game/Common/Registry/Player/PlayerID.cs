namespace Game.Common.Registry
{
    public struct PlayerID
    {
        public uint ID;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PlayerID))
            {
                return false;
            }

            return ((PlayerID) obj).ID == this.ID;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }
    }
}