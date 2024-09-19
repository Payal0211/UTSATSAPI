using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class ClientFeedbackViewModelValidator : AbstractValidator<ClientFeedbackViewModel>
    {
        public ClientFeedbackViewModelValidator()
        {
            RuleFor(x => x.HiringRequestID).NotNull().GreaterThan(0);
            RuleFor(x => x.ContactIDValue).NotNull().GreaterThan(0);
            RuleFor(x => x.TalentIDValue).NotNull().GreaterThan(0);
            //RuleFor(x => x.ShortlistedInterviewID).NotNull().GreaterThan(0);
            //RuleFor(x => x.CognitiveSkillRating).NotEmpty();
            //RuleFor(x => x.CommunicationSkillRating).NotEmpty();
            //RuleFor(x => x.TechnicalSkillRating).NotEmpty();
        }
    }
}
