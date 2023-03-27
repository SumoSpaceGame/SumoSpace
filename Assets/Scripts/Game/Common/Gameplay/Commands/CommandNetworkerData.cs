using Game.Common.Registry;

namespace Game.Common.Gameplay.Commands
{
    public class CommandNetworkerData
    {  
        
        private ICommandNetworker networker;
        
        private PlayerID id;
        public string commandType { get; private set; }
        
        public CommandNetworkerData(ICommandNetworker networker, PlayerID sendingPlayerID, string commandType)
        {
            this.id = sendingPlayerID;
            this.commandType = commandType;
            this.networker = networker;
        }

        public void Send()
        {
            Send(CommandPacketData.Empty());
        }

        public void Send(CommandPacketData data)
        {
            this.networker.SendData(data, commandType, id);
        }
        
    }
}