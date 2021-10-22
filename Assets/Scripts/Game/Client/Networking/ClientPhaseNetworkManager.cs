using BeardedManStudios.Forge.Networking.Generated;
using Game.Client.Phases;
using Game.Common.Phases;

namespace Game.Common.Networking
{
    /// <summary>
    /// Client phase network manager
    /// </summary>
    public partial class GamePhaseNetworkManager : GamePhaseBehavior
    {
        partial void ClientAddPhases()
        {
            this.gamePhases.Add(Phase.MATCH_CONNECT, new ClientPhaseMatchConnect(this));
            this.gamePhases.Add(Phase.MATCH_READY_UP, new ClientPhaseReadyUp(this));
            this.gamePhases.Add(Phase.MATCH_LOBBY, new ClientPhaseLobby(this));
            this.gamePhases.Add(Phase.MATCH_SYNC_LOAD_OUTS, new ClientPhaseSyncLoadout(this, masterSettings.playerGameDataRegistry, masterSettings.playerIDRegistry));
            this.gamePhases.Add(Phase.MATCH_LOAD_MAP, new ClientPhaseLoadMap(this));
            this.gamePhases.Add(Phase.MATCH_START_COUNTDOWN, new ClientPhaseStartMatch());
            this.gamePhases.Add(Phase.MATCH_GAME, new ClientPhaseGame());
        }
    }
}