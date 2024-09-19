using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class SaveBillPayRateViewModel_PayRateValidator : AbstractValidator<SaveBillPayRateViewModel>
    {
        public SaveBillPayRateViewModel_PayRateValidator()
        {
            RuleFor(x => x.BillrateCurrency).NotEmpty();
            RuleFor(x => x.PayRate).NotEmpty();
            RuleFor(x => x.payrateReason).NotEmpty();
        }
    }
}
