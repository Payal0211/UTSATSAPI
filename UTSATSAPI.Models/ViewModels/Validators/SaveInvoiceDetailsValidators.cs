using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class SaveInvoiceDetailsValidators : AbstractValidator<SaveInvoiceDetailsFromEngagementDashboard>
    {
        public SaveInvoiceDetailsValidators()
        {
            RuleFor(x => x.InvoiceSentdate).NotEmpty();
            RuleFor(x => x.InvoiceNumber).NotEmpty();
            RuleFor(x => x.InvoiceStatusId).NotEmpty();
            RuleFor(x => x.OnBoardID).NotEmpty();
        }
    }
}
