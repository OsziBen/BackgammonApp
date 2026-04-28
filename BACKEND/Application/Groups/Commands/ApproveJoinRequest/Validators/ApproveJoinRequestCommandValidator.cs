using FluentValidation;

namespace Application.Groups.Commands.ApproveJoinRequest.Validators
{
    public class ApproveJoinRequestCommandValidator : AbstractValidator<ApproveJoinRequestCommand>
    {
        public ApproveJoinRequestCommandValidator()
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
