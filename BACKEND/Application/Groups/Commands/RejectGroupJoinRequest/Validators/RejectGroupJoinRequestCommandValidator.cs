using FluentValidation;

namespace Application.Groups.Commands.RejectGroupJoinRequest.Validators
{
    public class RejectGroupJoinRequestCommandValidator : AbstractValidator<RejectGroupJoinRequestCommand>
    {
        public RejectGroupJoinRequestCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");

            RuleFor(x => x.RequestId)
               .NotEmpty()
               .WithMessage("Request ID is required.");
        }
    }
}
