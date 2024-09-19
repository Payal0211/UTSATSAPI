using FluentValidation;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class AddAMTeamValidator : AbstractValidator<AMTeam>
    {
        public AddAMTeamValidator()
        {
            RuleFor(x => x.GEOID).NotEmpty();
            RuleFor(x => x.SortNo).GreaterThan(0);
            RuleFor(x => x.AMId).NotEmpty();
        }
    }
}
