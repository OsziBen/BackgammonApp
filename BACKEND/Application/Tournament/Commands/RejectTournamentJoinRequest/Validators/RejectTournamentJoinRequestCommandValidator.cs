using Application.Tournament.Commands.RejectTournamentJoinRequest;
using FluentValidation;

namespace Application.Tournament.Commands.RejectTournamentRequest.Validators
{
    public class RejectTournamentJoinRequestCommandValidator : AbstractValidator<RejectTournamentJoinRequestCommand>
    {
        public RejectTournamentJoinRequestCommandValidator()
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
