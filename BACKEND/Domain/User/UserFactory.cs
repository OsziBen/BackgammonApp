using Common.Exceptions;

namespace Domain.User
{
    public static class UserFactory
    {
        public static User CreateAppUser(
            string firstName,
            string lastName,
            string userName,
            string emailAddress,
            string passwordHash,
            DateOnly dateOfBirth,
            Guid roleId,
            DateTimeOffset now)
        {
            if (dateOfBirth > DateOnly.FromDateTime(now.Date))
            {
                throw new BusinessRuleException("Invalid birth date");
            }

            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                EmailAddress = emailAddress,
                PasswordHash = passwordHash,
                DateOfBirth = dateOfBirth,
                ProfilePictureUrl = null,
                AppRoleId = roleId,
                Rating = 1500,
                ExperiencePoints = 0,
                IsBanned = false,
                IsDeleted = false,
                IsSystemUser = false,
                CreatedAt = now,
                LastUpdatedAt = now,
                DeletedAt = null
            };
        }
    }
}
