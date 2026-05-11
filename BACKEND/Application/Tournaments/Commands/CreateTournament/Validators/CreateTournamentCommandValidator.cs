using FluentValidation;

namespace Application.Tournaments.Commands.CreateTournament.Validators
{
    public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
    {
        public CreateTournamentCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.MaxParticipants)
                .GreaterThan(1)
                .LessThanOrEqualTo(1024);

            RuleFor(x => x.RulesTemplateId)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("Start date must be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");

            RuleFor(x => x.Deadline)
                .LessThanOrEqualTo(x => x.StartDate)
                .WithMessage("Deadline must be before or equal to start date.");

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.Visibility)
                .IsInEnum();
        }
    }
}
