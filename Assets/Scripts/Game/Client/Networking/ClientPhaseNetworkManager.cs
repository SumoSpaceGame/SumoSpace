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
        }
    }
}