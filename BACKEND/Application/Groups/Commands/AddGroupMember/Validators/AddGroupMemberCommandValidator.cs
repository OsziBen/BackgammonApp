using FluentValidation;

namespace Application.Groups.Commands.AddGroupMember.Validators
{
    public class AddGroupMemberCommandValidator : AbstractValidator<AddGroupMemberCommand>
    {
        public AddGroupMemberCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("User Name is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User Id is required.");
        }
    }
}
