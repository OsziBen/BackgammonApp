namespace Common.Enums
{
    public enum FunctionCode
    {
        // Általános hiba
        InternalError = 1000,

        // Validációs hibák (400 Bad Request)
        ValidationError = 2000,
        InvalidNameLength = 2001,
        InvalidPriceValue = 2002,

        // Erőforrás hibák (404 Not Found)
        ResourceNotFound = 3000,

        // Üzleti szabály hibák (409 Conflict / 422 Unprocessable Entity)
        BusinessRuleViolation = 4000,
        ArchivedItemModification = 4001,
        SessionFull = 4002,
        InvalidGamePhase = 4003,
        InvalidGameState = 4004,
        GameAlreadyFinished = 4005,
        NotYourTurn = 4006,
        InvalidMove = 4007,
        InvalidDiceRoll = 4007,
        InvalidDiceRollValues = 4008,

        // Jogosultsági/Hozzáférés hibák (403 Forbidden)
        AccessDenied = 5000,
        MissingPermission = 5001
    }
}
