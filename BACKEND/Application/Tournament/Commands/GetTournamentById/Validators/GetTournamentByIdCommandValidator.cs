using FluentValidation;

namespace Application.Tournament.Commands.GetTournamentById.Validators
{
    public class GetTournamentByIdCommandValidator : AbstractValidator<GetTournamentByIdCommand>
    {
        public GetTournamentByIdCommandValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEmpty()
                .WithMessage("Tournament ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
