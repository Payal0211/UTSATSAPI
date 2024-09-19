using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class SaveBillPayRateViewModel_BillRateValidator : AbstractValidator<SaveBillPayRateViewModel>
    {
        public SaveBillPayRateViewModel_BillRateValidator()
        {
            RuleFor(x => x.BillrateCurrency).NotEmpty();
            RuleFor(x => x.BillRate).NotEmpty();
            RuleFor(x => x.BillRateReason).NotEmpty();
        }
    }
}
