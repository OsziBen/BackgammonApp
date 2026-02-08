namespace WebAPI.Constants.ApiRouteConstants
{
    public static class GameSessionConstants
    {
        public const string Base = "game-sessions";
        public const string ActiveByUserId = "active/{userId:guid}";    // TODO: később my-active
        public const string ById = "{sessionId:guid}";
    }
}
