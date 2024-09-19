namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.ResponseModels;
    public class PrimaryClientModelValidator : AbstractValidator<Primaryclient>
    {
        public PrimaryClientModelValidator()
        {
            RuleFor(x => x.fullName).NotEmpty();
            RuleFor(x => x.linkedin).NotEmpty();
            RuleFor(x => x.emailId).NotEmpty().EmailAddress();
        }
    }
}
