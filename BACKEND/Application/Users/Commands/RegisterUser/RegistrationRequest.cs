namespace Application.Users.Commands.RegisterUser
{
    public class RegistrationRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public required DateOnly DateOfBirth { get; set; }
    }
}
