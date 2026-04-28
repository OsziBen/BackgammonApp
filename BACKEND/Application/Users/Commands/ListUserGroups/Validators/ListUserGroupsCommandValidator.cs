using FluentValidation;

namespace Application.Users.Commands.ListUserGroups.Validators
{
    public class ListUserGroupsCommandValidator : AbstractValidator<ListUserGroupsCommand>
    {
        public ListUserGroupsCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
