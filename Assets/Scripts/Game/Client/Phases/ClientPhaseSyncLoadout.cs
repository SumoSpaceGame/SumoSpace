using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;

namespace Game.Client.Phases
{
    public class ClientPhaseSyncLoadout : IGamePhase
    {
        public ClientPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
        }

        public void PhaseStart()
        {
            //Start timer dependent on time length
        }

        public void PhaseUpdate()
        {
        }


        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            var syncData = PhaseSyncLoadout.Decode(data);
            
            //Update ui to show syncData
            
        }
    }
}