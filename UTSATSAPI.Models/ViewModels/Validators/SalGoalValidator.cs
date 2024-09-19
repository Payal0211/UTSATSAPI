using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class SalGoalValidator : AbstractValidator<SalGoal_INR>
    {
        public SalGoalValidator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.BDR_INR).NotEmpty();
            RuleFor(x => x.BDRLead_INR).NotEmpty();
        }
    }

    public class SalGoal_FirstClosureValidator : AbstractValidator<SalGoal_USD_FirstClosure>
    {
        public SalGoal_FirstClosureValidator() 
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SalesConsultant_USD).NotEmpty();
            RuleFor(x => x.PODManagers_USD).NotEmpty();
            RuleFor(x => x.BDR_USD).NotEmpty();
            RuleFor(x => x.BDRLead_USD).NotEmpty();
            RuleFor(x => x.BDRManagerHead_USD).NotEmpty();
            RuleFor(x => x.MarketingTeam_USD).NotEmpty();
            RuleFor(x => x.MarketingLead_USD).NotEmpty();
            RuleFor(x => x.MarketingHead_USD).NotEmpty();
            RuleFor(x => x.AM_USD).NotEmpty();
            RuleFor(x => x.AMHead_USD).NotEmpty();
        }
    }
}
