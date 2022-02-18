using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
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
        
        public GameMatchSettings gameMatchSettings;
        
        
        private UDPClient _gameClient;
        private UDPServer _gameServer;
    
        // Start is called before the first frame update
        void Awake()
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
            Debug.Log("Connecting to game server.." + address + " " + port);
    
            gameMatchSettings.Reset();

            _gameClient = new UDPClient();

            _gameClient.serverAccepted += (sender) =>
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
            Debug.Log("Starting game server.. " + address + " : " + port);
        
            _gameServer = new UDPServer(gameMatchSettings.PlayerCount + 1);
            
            
            _gameServer.playerConnected += (player, sender) => Debug.Log("Player connected into server!");
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
            
            NetworkManager.Instance.InstantiateMatchTimer();
            NetworkManager.Instance.InstantiateGameManager();
            NetworkManager.Instance.InstantiateAgentManager();
            NetworkManager.Instance.InstantiateInputLayer();
            NetworkManager.Instance.InstantiateGamePhase();
        }

    }
}
