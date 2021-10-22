namespace Game.Common.Phases
{
    /// <summary>
    /// Phases represent the state of the match. This list is in chronological order.
    /// </summary>
    public enum Phase
    {
        MATCH_CONNECT, // Waiting for players
        MATCH_READY_UP,  // Ready up phase
        MATCH_SYNC_PLAYER_DATA,
        MATCH_LOBBY, // Lobby selection phase   
        MATCH_SYNC_LOAD_OUTS, // Sync load outs between players (Start point of loading screen) (Also gives the players 5 seconds until starting)
        MATCH_LOAD_MAP, // Sends map information to players, and waits for everyone to be loaded
        MATCH_START_COUNTDOWN, // Start countdown and send them into the match 
        MATCH_GAME, // Game phase
        MATCH_FINISH_GAME, // Finish game phase
        MATCH_SCOREBOARD_SYNC, // Scoreboard sent to all players and database
        MATCH_STOP // Stop match
            
    }
}