using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class AMSlabValidator : AbstractValidator<UpdateAMSlab>
    {
        public AMSlabValidator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.AM).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
            RuleFor(x => x.AMHead).NotEmpty().GreaterThan(0).InclusiveBetween(0, 100);
        }
    }
}
