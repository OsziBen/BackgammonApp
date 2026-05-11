using FluentValidation;

namespace Application.Tournaments.Commands.UpdateTournament.Validators
{
    public class UpdateTournamentCommandValidator : AbstractValidator<UpdateTournamentCommand>
    {
        public UpdateTournamentCommandValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEmpty()
                .WithMessage("Tournament id is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tournament name is required.")
                .MaximumLength(100)
                .WithMessage("Tournament name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Tournament description is required.")
                .MaximumLength(500)
                .WithMessage("Tournament description cannot exceed 1000 characters.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid tournament type.");

            RuleFor(x => x.Visibility)
                .IsInEnum()
                .WithMessage("Invalid tournament visibility.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid tournament status.");

            RuleFor(x => x.MaxParticipants)
                .GreaterThan(1)
                .WithMessage("Tournament must allow at least 2 participants.")
                .LessThanOrEqualTo(30)
                .WithMessage("Tournament participant limit is too high.");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("Start date must be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after the start date.");

            RuleFor(x => x.Deadline)
                .LessThan(x => x.StartDate)
                .WithMessage("Registration deadline must be before the start date.");

            RuleFor(x => x.RulesTemplateId)
                .NotEmpty()
                .WithMessage("Rules template id is required.");
        }
    }
}