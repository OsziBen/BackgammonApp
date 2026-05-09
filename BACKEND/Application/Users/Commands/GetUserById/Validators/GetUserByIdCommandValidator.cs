using FluentValidation;

namespace Application.Users.Commands.GetUserById.Validators
{
    public class GetUserByIdCommandValidator : AbstractValidator<GetUserByIdCommand>
    {
        public GetUserByIdCommandValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
        }
    }
}
