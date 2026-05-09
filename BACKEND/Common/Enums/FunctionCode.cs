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
        // GAME SESSION
        SessionFull = 4002,
        InvalidGamePhase = 4003,
        InvalidGameState = 4004,
        GameAlreadyFinished = 4005,
        NotYourTurn = 4006,
        InvalidMove = 4007,
        InvalidDiceRoll = 4008,
        InvalidDiceRollValues = 4009,
        PlayerNotInSession = 4010,
        DiceAlreadyRolled = 4011,
        InsufficientPlayerNumber = 4012,
        PlayerDoesNotPossessDoublingCube = 4013,
        CrawfordRuleInEffect = 4014,
        DoublingCubeDisabled = 4015,
        InvalidPlayer = 4016,
        SessionAlreadyStarted = 4017,
        // USERS
        UserAlreadyInActiveSession = 4018,
        UserWithEmailAlreadyExists = 4019,
        UserWithUserNameAlreadyExists = 4020,
        CannotRollDice = 4021,
        // GROUPS
        GroupWithGroupNameAlreadyExists = 4022,
        CannotDowngradeGroupSize = 4023,
        CannotAddUserDirectlyInPublicGroup = 4024,
        UserAlreadyActiveMember = 4025,
        GroupReachedMaxMembersLimit = 4026,
        CannotRemoveGroupOwner = 4027,
        InvalidJoinRequestStatus = 4028,
        GroupMismatch = 4029,
        CannotDemoteGroupOwner = 4030,
        UserIsNotModerator = 4031,
        ModeratorLimitReached = 4032,
        UserIsAlreadyModerator = 4033,
        CannotJoinPrivateGroup = 4034,
        JoinAlreadyRequested = 4035,
        // TOURNAMENTS
        CannotAddParticipantDirectlyInPublicTournament = 4036,
        UserAlreadyActiveParticipant = 4037,
        TournamentReachedMaxParticipantsLimit = 4038,
        JoinDeadlinePassed = 4039,
        TournamentMismatch = 4040,
        TournamentWithTournamentNameAlreadyExists = 4041,
        MaxParicipantsCannotBeLessThanCurrentNumberOfParticipants = 4042,
        CannotRemoveTournamentOrganizer = 4043,
        CannotJoinPrivateTournament = 4044,


        // Jogosultsági/Hozzáférés hibák (403 Forbidden)
        AccessDenied = 5000,
        Unauthorized = 5001,
        MissingPermission = 5002
    }
}
