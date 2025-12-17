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

        // Jogosultsági/Hozzáférés hibák (403 Forbidden)
        AccessDenied = 5000,
        MissingPermission = 5001
    }
}
