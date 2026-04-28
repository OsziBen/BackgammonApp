using FluentValidation;

namespace Application.Groups.Commands.GetAllGroups.Validators
{
    public class GetAllPublicGroupsCommandValidator : AbstractValidator<GetAllPublicGroupsCommand>
    {
        public GetAllPublicGroupsCommandValidator()
        {
        }
    }
}
