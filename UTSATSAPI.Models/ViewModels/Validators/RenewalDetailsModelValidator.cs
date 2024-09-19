using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class RenewalDetailsModelValidator : AbstractValidator<RenewalDetailsModel>
    {
        public RenewalDetailsModelValidator()
        {
            RuleFor(x => x.OnBoardId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ContractStartDate).NotEmpty();
            //RuleFor(x => x.ContractEndDate).NotEmpty();
            //RuleFor(x => x.ContarctDuration).NotEmpty();
            RuleFor(x => x.BillRate).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PayRate).NotEmpty().GreaterThan(0);
            RuleFor(x => x.NRPercentage).NotEmpty();
        }
    }
}