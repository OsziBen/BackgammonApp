using FluentValidation;

namespace Application.Groups.Commands.ApproveGroupJoinRequest.Validators
{
    public class ApproveGroupJoinRequestCommandValidator : AbstractValidator<ApproveGroupJoinRequestCommand>
    {
        public ApproveGroupJoinRequestCommandValidator()
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
