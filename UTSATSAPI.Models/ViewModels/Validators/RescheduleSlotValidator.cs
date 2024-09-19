namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class RescheduleSlotValidator : AbstractValidator<RescheduleSlot>
    {
        public RescheduleSlotValidator()
        {
            RuleFor(x => x.STRSlotDate).NotNull().NotEmpty();
            RuleFor(x => x.STRStartTime).NotNull().NotEmpty();
            RuleFor(x => x.STREndTime).NotNull().NotEmpty();
            RuleFor(x => x.SlotDate).NotNull().NotEmpty();
            RuleFor(x => x.StartTime).NotNull().NotEmpty();
            RuleFor(x => x.EndTime).NotNull().NotEmpty();
        }
    }
}
