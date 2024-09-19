
namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class ScheduleInterviewModelValidator : AbstractValidator<ScheduleInterviewModel>
    {
        public ScheduleInterviewModelValidator()
        {
            RuleFor(x => x.SlotType).NotEmpty();
            RuleFor(x => x.RecheduleSlots).NotNull();
            RuleFor(x => x.HiringRequest_ID).NotEmpty();
            RuleFor(x => x.HiringRequest_Detail_ID).NotEmpty();
            RuleFor(x => x.ContactID).NotEmpty();
            RuleFor(x => x.Talent_ID).NotEmpty();
            RuleFor(x => x.InterviewMasterID).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WorkingTimeZoneID).NotEmpty();
        }
    }
}
