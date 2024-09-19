using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class OnBoardViewModelValidators : AbstractValidator<OnBoardViewModel>
    {
        public OnBoardViewModelValidators()
        {
            
            RuleFor(x => x.ClientName).NotEmpty();
            
            RuleFor(x => x.Clientemail).NotEmpty();
            
            RuleFor(x => x.TalentName).NotEmpty();
            
            RuleFor(x => x.EngagemenID).NotEmpty();
            
            RuleFor(x => x.HiringRequestNumber).NotEmpty();
            
            RuleFor(x => x.ContractType).NotEmpty();
            
            RuleFor(x => x.EngagemenID).NotEmpty();
            
            RuleFor(x => x.ContractDuration).NotEmpty();

            RuleFor(x => x.ContractStartDate).NotEmpty();

            RuleFor(x => x.ContractEndDate).NotEmpty();

            RuleFor(x => x.TotalDurationInMonths).NotEmpty();

            RuleFor(x => x.TalentWorkingTimeZone).NotEmpty();

            RuleFor(x => x.TalentWorkingTimeZone).NotEmpty();

            RuleFor(x => x.PunchStartTime).NotEmpty();

            RuleFor(x => x.PunchEndTime).NotEmpty();

            RuleFor(x => x.WokringDays).NotEmpty();

            RuleFor(x => x.WokringDays).NotEmpty();

            RuleFor(x => x.TalentOnBoardDate).NotEmpty();

            RuleFor(x => x.TalentOnBoardTime).NotEmpty();
        }
    }
}
