namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class ListOfTalentsValidators : AbstractValidator<ListOfTalents>
    {
        public ListOfTalentsValidators()
        {
            RuleFor(x => x.talentId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.amount).NotNull().GreaterThan(0);
        }
    }
}
