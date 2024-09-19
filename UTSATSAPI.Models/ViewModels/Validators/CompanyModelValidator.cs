
namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.ResponseModels;

    public class CompanyModelValidator : AbstractValidator<Company>
    {
        public CompanyModelValidator()
        {
            RuleFor(x => x.company).NotEmpty();
            RuleFor(x => x.website).NotEmpty();
            RuleFor(x => x.location).NotEmpty();
            RuleFor(x => x.companySize).NotEmpty();
            RuleFor(x => x.address).NotEmpty();
            RuleFor(x => x.linkedinProfile).NotEmpty();
        }
    }
}
