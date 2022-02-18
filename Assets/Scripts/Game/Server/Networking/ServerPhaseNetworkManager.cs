using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Client.SceneLoading;
using Game.Common.Instances;
using Game.Common.Networking.Misc;
using Game.Common.Phases;
using Game.Server.Phases;
using UnityEngine.InputSystem.LowLevel;

namespace Game.Common.Networking
{
    /// <summary>
    /// Server phase network manager
    /// </summary>
    public partial class GamePhaseNetworkManager : GamePhaseBehavior
    {
        /// <summary>
        /// Adds the specific server phase classes to be used in the manager
        /// </summary>
        partial void ServerAddPhases()
        {
            this.gamePhases.Add(Phase.MATCH_CONNECT, new ServerPhaseMatchConnect(this));
            this.gamePhases.Add(Phase.MATCH_READY_UP, new ServerPhaseReadyUp(this));
            this.gamePhases.Add(Phase.MATCH_LOBBY, new ServerPhaseLobby(this));
            this.gamePhases.Add(Phase.MATCH_SYNC_LOAD_OUTS, new ServerPhaseSyncLoadout(this));
            this.gamePhases.Add(Phase.MATCH_LOAD_MAP, new ServerPhaseLoadMap(this, MainPersistantInstances.Get<SceneLoader>()));
            this.gamePhases.Add(Phase.MATCH_START_COUNTDOWN, new ServerPhaseStartMatch(this, MainPersistantInstances.Get<MatchNetworkTimerManager>()));
            this.gamePhases.Add(Phase.MATCH_GAME, new ServerPhaseGame());
        }

        /// <summary>
        /// Checks on phase every frame
        /// </summary>
        partial void ServerUpdate()
        {
            var curPhase = GetCurrentPhase();
        }


        /// <summary>
        /// Switches the server's phase to next, and also tells clients to do so too.
        /// </summary>
        public void ServerNextPhase()
        {
            this._gamePhaseManager.NextPhase();
            this.networkObject.SendRpc(RPC_SWITCH_PHASE, Receivers.Others, (int) this._gamePhaseManager.CurrentPhase);
        }
        
        /// <summary>
        /// Switches the server phase to specified phrase, and also tells clients to do so too.
        /// </summary>
        /// <param name="phase"></param>
        public void ServerSwitchPhase(Phase phase)
        {
            this._gamePhaseManager.SwitchPhase(phase);
            this.networkObject.SendRpc(RPC_SWITCH_PHASE, Receivers.Others, (int) phase);
        }
        
        
    }
}