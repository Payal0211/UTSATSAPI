namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels;


    public class DirectPlacementValidator : AbstractValidator<DirectPlacementViewModel>
    {
        public DirectPlacementValidator()
        {
            //RuleFor(x => x.City).NotEmpty().NotNull().WithMessage("Please enter city.");
            //RuleFor(x => x.Country).NotNull().NotEmpty().NotEqual("0").WithMessage("Please select the country.");
        }
    }

    //public class DirectHr_DirectPlacementValidator : AbstractValidator<DirectPlacementViewModel>
    //{
    //    public DirectHr_DirectPlacementValidator()
    //    {
    //        RuleFor(x => x.City).NotEmpty().NotNull().WithMessage("Please enter city.");
    //        RuleFor(x => x.Country).NotNull().NotEmpty().NotEqual("0").WithMessage("Please select the country.");
    //    }
    //}
}
