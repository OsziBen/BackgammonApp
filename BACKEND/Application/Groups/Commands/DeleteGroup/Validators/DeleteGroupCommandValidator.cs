using FluentValidation;

namespace Application.Groups.Commands.DeleteGroup.Validators
{
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithMessage("Group ID is required.");
        }
    }
}
