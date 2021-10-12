using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine.SceneManagement;

namespace Game.Client.Phases
{
    public class ClientPhaseLoadMap : IGamePhase
    {

        private GamePhaseNetworkManager _phaseNetworkManager;
        public ClientPhaseLoadMap(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestMap");
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOAD_MAP, new byte[]{});
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
        }
    }
}