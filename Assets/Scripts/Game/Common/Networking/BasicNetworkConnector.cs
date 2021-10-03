using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Agents;
using Game.Common.Instances;
using Game.Common.Phases;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Game.Common.Networking
{
    /// <summary>
    /// This is temporary class to be able to connect to a server, will be deprecated.
    /// </summary>
    public class BasicNetworkConnector : MonoBehaviour
    {
    
        public GameObject networkManagerPrefab;

        private UDPClient _gameClient;
        private UDPServer _gameServer;
    
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            return;
            /*
            if (NetworkManager.Instance != null && !NetworkManager.Instance.IsServer)
            {
                // ReSharper disable once ReplaceWithSingleAssignment.True
                bool allSystemsSet = true;
                
                allSystemsSet &= MainInstances.Get<GamePhaseNetworkManager>() == null;
                allSystemsSet &= MainInstances.Get<AgentManager>() == null;
                allSystemsSet &= MainInstances.Get<GameNetworkManager>() == null;
                
                if (allSystemsSet)
                {
                    SceneManager.LoadScene(1);
                }

            }*/
        }
    
        public void Connect(string address, ushort port)
        {
            Debug.Log("Connecting to game server..");
        
            _gameClient = new UDPClient();
        
            _gameClient.playerConnected += (player, sender) =>
            {
                Debug.Log("Player Connected!");
                
                NetworkObject.Flush(_gameClient);
            };
            _gameClient.connectAttemptFailed += (sender) => Debug.Log("Connection Failed!");

            _gameClient.Connect(address, port);
        

            //If network manager does not exist, make sure to spawn it in. 
            if (NetworkManager.Instance == null)
            {
                Instantiate(networkManagerPrefab);
            }
        
            Rpc.MainThreadRunner = MainThreadManager.Instance;
        
            NetworkManager.Instance.Initialize(_gameClient);
        }
        
        public void Host(string address, ushort port)
        {
            Debug.Log("Starting game server..");
        
            _gameServer = new UDPServer(GameNetworkManager.MATCH_PLAYER_SIZE + 1);
        

            _gameServer.playerAccepted += (player, sender) => Debug.Log("Player accepted into server!");
            _gameServer.playerDisconnected += (player, sender) => Debug.Log("Player disconnected from server!");


            _gameServer.Connect(address, port);
            
            //If network manager does not exist, make sure to spawn it in. 
            if (NetworkManager.Instance == null)
            {
                Instantiate(networkManagerPrefab);
            }
        
            Rpc.MainThreadRunner = MainThreadManager.Instance;
        
            NetworkManager.Instance.Initialize(_gameServer);
            NetworkObject.Flush(_gameServer);
            Debug.Log("Loading new scene");
            //SceneManager.LoadScene(1);
            //SceneManager.sceneLoaded += NetworkInstantiate;
            //Debug.Log("Instantiating required objects");
            
            NetworkManager.Instance.InstantiateGameManager();
            NetworkManager.Instance.InstantiateAgentManager();
            NetworkManager.Instance.InstantiateGamePhase();
        }

        public static void NetworkInstantiate(Scene scene, LoadSceneMode mode)
        {
            
            NetworkManager.Instance.InstantiateGameManager();
            NetworkManager.Instance.InstantiateAgentManager();
            NetworkManager.Instance.InstantiateGamePhase();

            SceneManager.sceneLoaded -= NetworkInstantiate;
        }
    }
}
