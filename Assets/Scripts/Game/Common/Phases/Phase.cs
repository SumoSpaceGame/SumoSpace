namespace Game.Common.Phases
{
    /// <summary>
    /// Phases represent the state of the match. This list is in chronological order.
    /// </summary>
    public enum Phase
    {
        MATCH_CONNECT = 0, // Waiting for players
        MATCH_READY_UP = 1,  // Ready up phase
        MATCH_LOBBY = 2, // Lobby selection phase   
        MATCH_SYNC_LOAD_OUTS = 3, // Sync load outs between players (Start point of loading screen) (Also gives the players 5 seconds until starting)
        MATCH_LOAD_MAP = 4, // Sends map information to players, and waits for everyone to be loaded
        MATCH_START_COUNTDOWN = 5, // Start countdown and send them into the match 
        MATCH_GAME = 6, // Game phase
        MATCH_FINISH_GAME = 7, // Finish game phase
        MATCH_SCOREBOARD_SYNC = 8, // Scoreboard sent to all players and database
        MATCH_STOP = 9 // Stop match
            
    }
}