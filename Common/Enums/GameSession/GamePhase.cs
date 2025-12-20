namespace Common.Enums.GameSession
{
    public enum GamePhase
    {
        WaitingForPlayers = 0,
        DeterminingStartingPlayer = 1,

        RollDice = 10,
        MoveCheckers = 11,

        CubeOffered = 20,
        CubeAccepted = 21,
        CubeDeclined = 22,

        GameAbandoned = 98,
        GameFinished = 99
    }

}
