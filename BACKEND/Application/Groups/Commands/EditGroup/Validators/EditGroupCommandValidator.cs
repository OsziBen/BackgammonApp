using FluentValidation;

namespace Application.Groups.Commands.EditGroup.Validators
{
    public class EditGroupCommandValidator : AbstractValidator<EditGroupCommand>
    {
        public EditGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Group name is required.")
                .MaximumLength(100)
                .WithMessage("Group name cannot exceed 100 characters.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Group name cannot be empty or whitespace.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(200)
                .WithMessage("Description cannot exceed 200 characters.")
                .Must(desc => !string.IsNullOrWhiteSpace(desc))
                .WithMessage("Description cannot be empty or whitespace.");

            RuleFor(x => x.Visibility)
                .IsInEnum()
                .WithMessage("Invalid visibility type.");

            RuleFor(x => x.SizePreset)
                .IsInEnum()
                .WithMessage("Invalid group size preset.");
        }
    }
}
