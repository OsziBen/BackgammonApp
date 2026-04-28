using FluentValidation;

namespace Application.Groups.Commands.LeaveGroup.Validators
{
    public class LeaveGroupCommandValidator : AbstractValidator<LeaveGroupCommand>
    {
        public LeaveGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
