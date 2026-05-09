using FluentValidation;

namespace Application.Groups.Commands.GetAllPublicGroups.Validators
{
    public class GetAllPublicGroupsCommandValidator : AbstractValidator<GetAllPublicGroupsCommand>
    {
        public GetAllPublicGroupsCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
