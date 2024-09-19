namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.ResponseModels;
    public class SecondaryValidator :  AbstractValidator<Secondaryclient>
    {
        public SecondaryValidator()
        {
            RuleFor(x => x.fullName).NotEmpty();
            RuleFor(x => x.emailID).NotEmpty().EmailAddress();
            RuleFor(x => x.linkedinProfile).NotEmpty();

        }
    }
}
