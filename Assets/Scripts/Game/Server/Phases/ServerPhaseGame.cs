using FishNet.Connection;
using Game.Common.Phases;

namespace Game.Server.Phases
{
    public class ServerPhaseGame : IGamePhase
    {
        public void PhaseStart()
        {
            
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
        }
    }
}