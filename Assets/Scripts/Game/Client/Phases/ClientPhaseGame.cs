using BeardedManStudios.Forge.Networking;
using FishNet.Connection;
using Game.Common.Phases;

namespace Game.Client.Phases
{
    public class ClientPhaseGame : IGamePhase
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