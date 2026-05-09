using FluentValidation;

namespace Application.Tournament.Commands.ApproveTournamentJoinRequest.Validators
{
    public class ApproveTournamentJoinRequestCommandValidator : AbstractValidator<ApproveTournamentJoinRequestCommand>
    {
        public ApproveTournamentJoinRequestCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Tournament ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");

            RuleFor(x => x.RequestId)
               .NotEmpty()
               .WithMessage("Request ID is required.");
        }
    }
}
