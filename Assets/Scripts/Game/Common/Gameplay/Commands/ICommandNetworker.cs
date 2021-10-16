namespace Game.Common.Gameplay.Commands
{
    public interface ICommandNetworker
    {
        bool SendData(CommandPacketData data);
    }
}