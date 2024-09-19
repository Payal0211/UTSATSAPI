using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class PlacementFeesValidator : AbstractValidator<UpdatePlacementFeesModel>
    {
        public PlacementFeesValidator() 
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SalesConsultant).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.PODManagers).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.BDR).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.BDR_Lead).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.BDRManager_Head).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingTeam).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingLead).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingHead).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.AM).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.AMHead).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
        }
    }

    public class Contract_Validator : AbstractValidator<Update_ContractModel>
    {
        public Contract_Validator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SalesConsultant).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.PODManagers).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.BDR_USD).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.BDRLead_USD).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.BDRManagerHead_USD).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingTeam).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingLead).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.MarketingHead).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.AM).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.AMHead).NotEmpty().InclusiveBetween(0, 100);
        }
    }
}
