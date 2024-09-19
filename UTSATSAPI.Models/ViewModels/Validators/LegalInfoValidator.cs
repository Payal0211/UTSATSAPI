namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.ResponseModels;
    public class LegalInfoValidator : AbstractValidator<Legalinfo>
    {
        public LegalInfoValidator()
        {
            RuleFor(x => x.name).NotEmpty();
            RuleFor(x => x.email).NotEmpty().EmailAddress();
            RuleFor(x => x.legalCompanyName).NotEmpty();
            RuleFor(x => x.legalCompanyAddress).NotEmpty();
        }
    }
}
