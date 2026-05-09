using FluentValidation;

namespace Application.Groups.Commands.GetGroupById.Validators
{
    public class GetGroupByIdCommandValidator : AbstractValidator<GetGroupByIdCommand>
    {
        public GetGroupByIdCommandValidator()
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
