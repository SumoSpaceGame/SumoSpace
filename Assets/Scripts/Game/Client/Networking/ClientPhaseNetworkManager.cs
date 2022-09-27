using FishNet.Object;
using Game.Client.Phases;
using Game.Client.SceneLoading;
using Game.Common.Instances;
using Game.Common.Phases;

namespace Game.Common.Networking
{
    /// <summary>
    /// Client phase network manager
    /// </summary>
    public partial class GamePhaseNetworkManager : NetworkBehaviour
    {
        partial void ClientAddPhases()
        {
            this.gamePhases.Add(Phase.MATCH_CONNECT, new ClientPhaseMatchConnect(this));
            this.gamePhases.Add(Phase.MATCH_READY_UP, new ClientPhaseReadyUp(this));
            this.gamePhases.Add(Phase.MATCH_SYNC_PLAYER_DATA, new ClientPhaseSyncPlayerData(this));
            this.gamePhases.Add(Phase.MATCH_LOBBY, new ClientPhaseLobby(this));
            this.gamePhases.Add(Phase.MATCH_SYNC_LOAD_OUTS, new ClientPhaseSyncLoadout(this, masterSettings.playerGameDataRegistry, masterSettings.playerIDRegistry));
            this.gamePhases.Add(Phase.MATCH_LOAD_MAP, new ClientPhaseLoadMap(this, MainPersistantInstances.Get<SceneLoader>()));
            this.gamePhases.Add(Phase.MATCH_START_COUNTDOWN, new ClientPhaseStartMatch(this));
            this.gamePhases.Add(Phase.MATCH_GAME, new ClientPhaseGame());
        }
    }
}