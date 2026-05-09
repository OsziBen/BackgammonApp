using FluentValidation;

namespace Application.Tournament.Commands.ListTournamentJoinRequests.Validators
{
    public class ListTournamentJoinRequestsCommandValidator : AbstractValidator<ListTournamentJoinRequestsCommand>
    {
        public ListTournamentJoinRequestsCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Tournament ID is required.");
        }
    }
}
