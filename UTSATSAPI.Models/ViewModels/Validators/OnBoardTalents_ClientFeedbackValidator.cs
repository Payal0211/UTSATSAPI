using FluentValidation;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class OnBoardTalents_ClientFeedbackValidator : AbstractValidator<OnBoardTalents_ClientFeedback>
    {
        public OnBoardTalents_ClientFeedbackValidator()
        {
            RuleFor(x => x.FeedbackType).NotEmpty();
            RuleFor(x => x.FeedbackCreatedDateTime).NotEmpty();
            RuleFor(x => x.FeedbackComment).NotEmpty();
        }
    }
}
