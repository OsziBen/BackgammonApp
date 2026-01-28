namespace Common.Enums.GameSession
{
    public enum GamePhase
    {
        WaitingForPlayers = 0,
        DeterminingStartingPlayer = 1,

        TurnStart = 9,
        RollDice = 10,
        MoveCheckers = 11,

        CubeOffered = 20,

        GameAbandoned = 98,
        GameFinished = 99
    }

}
