using FluentValidation;

namespace Application.Groups.Commands.RejectJoinRequest.Validators
{
    public class RejectJoinRequestCommandValidator : AbstractValidator<RejectJoinRequestCommand>
    {
        public RejectJoinRequestCommandValidator()
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
