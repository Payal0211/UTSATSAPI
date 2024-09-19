namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
    public class DeleteHRValidator : AbstractValidator<DeleteHR>
    {
        public DeleteHRValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.DeleteType).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Remark).NotEmpty();
            RuleFor(x => x.ReasonId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
