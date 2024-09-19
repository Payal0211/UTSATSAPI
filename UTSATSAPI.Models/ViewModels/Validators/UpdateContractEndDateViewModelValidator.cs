using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class UpdateContractEndDateViewModelValidator : AbstractValidator<UpdateContractEndDateViewModel>
    {
        public UpdateContractEndDateViewModelValidator()
        {
            RuleFor(x => x.ContractEndDate).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
