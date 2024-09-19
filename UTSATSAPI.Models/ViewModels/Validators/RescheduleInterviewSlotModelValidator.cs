
namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class RescheduleInterviewSlotModelValidator : AbstractValidator<RescheduleInterviewSlotModel>
    {
        public RescheduleInterviewSlotModelValidator()
        {
            RuleFor(x => x.SlotType).NotEmpty();
            RuleFor(x => x.RescheduleSlot).NotNull();
            RuleFor(x => x.HiringRequest_ID).NotEmpty();
            RuleFor(x => x.HiringRequest_Detail_ID).NotEmpty();
            RuleFor(x => x.ContactID).NotEmpty();
            RuleFor(x => x.Talent_ID).NotEmpty();
            RuleFor(x => x.InterviewMasterID).NotEmpty();
            RuleFor(x => x.WorkingTimeZoneID).NotEmpty();
            //RuleFor(x => x.RescheduleRequestBy).NotEmpty();
            RuleFor(x => x.InterviewStatus).NotEmpty();
            RuleFor(x => x.HiringRequestNumber).NotEmpty();
            //RuleFor(x => x.NextRound_InterviewDetailsID).NotEmpty();

        }
    }
}
