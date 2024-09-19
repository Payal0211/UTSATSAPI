namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class interviewerValidator : AbstractValidator<InterviewerClass>
    {
        public interviewerValidator()
        {
            RuleFor(x => x.emailID).NotEmpty().EmailAddress();
            RuleFor(x => x.linkedin).NotEmpty();
            RuleFor(x => x.fullName).NotEmpty();
            RuleFor(x => x.designation).NotEmpty();
        }
    }
}
