namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class HiringReqeustModelValidator : AbstractValidator<HiringReqeustModel>
    {
        public HiringReqeustModelValidator()
        {
            RuleFor(x => x.clientName).NotEmpty().NotEmpty();
            RuleFor(x => x.companyName).NotEmpty();
            RuleFor(x => x.IsTransparentPricing).NotNull();
            RuleFor(x => x.salesPerson).NotEmpty().NotNull();
            RuleFor(x => x.availability).NotEmpty();
            RuleFor(x => x.Currency).MaximumLength(10);
            RuleFor(x => x.BudgetType).NotEmpty().NotNull();
            RuleFor(x => x.years).NotNull().InclusiveBetween(0, 100);
            RuleFor(x => x.talentsNumber).NotEmpty();
            RuleFor(x => x.contactId).GreaterThan(0);
            RuleFor(x => x.TimeZoneFromTime).NotEmpty().NotNull();
            RuleFor(x => x.TimeZoneEndTime).NotEmpty().NotNull();
            RuleFor(x => x.timeZone).NotEmpty().NotNull();
            RuleFor(x => x.howsoon).NotEmpty().NotNull();
            RuleFor(x => x.dealID)
               .Custom((x, context) =>
               {
                   if ((!(long.TryParse(x, out long value)) || value < 0))
                   {
                       context.AddFailure($"{x} is not a valid number or less than 0");
                   }
               });
            RuleFor(x => x.Currency).MaximumLength(10);
        }
    }
    public class DirectHR_or_CreditHR_ModelValidator : AbstractValidator<HiringReqeustModel>
    {
        public DirectHR_or_CreditHR_ModelValidator()
        {
            RuleFor(x => x.clientName).NotEmpty().NotEmpty();
            RuleFor(x => x.companyName).NotEmpty();
            RuleFor(x => x.salesPerson).NotEmpty().NotNull();
            RuleFor(x => x.availability).NotEmpty();
            RuleFor(x => x.Currency).MaximumLength(10);
            RuleFor(x => x.BudgetType).NotEmpty().NotNull();
            RuleFor(x => x.years).NotNull().InclusiveBetween(0, 100);
            RuleFor(x => x.talentsNumber).NotEmpty();
            RuleFor(x => x.contactId).GreaterThan(0);
            RuleFor(x => x.TimeZoneFromTime).NotEmpty().NotNull();
            RuleFor(x => x.TimeZoneEndTime).NotEmpty().NotNull();
            RuleFor(x => x.timeZone).NotEmpty().NotNull();
            RuleFor(x => x.howsoon).NotEmpty().NotNull();
        }
    }
}
