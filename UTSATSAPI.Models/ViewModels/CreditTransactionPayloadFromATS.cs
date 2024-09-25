using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CreditTransactionPayloadFromATS
    {
        public long? company_id { get; set; }
        public long? contact_id { get; set; }
        public long? hr_id { get; set; }
        public long? atstalent_id { get; set; }
        public string? transaction_type { get; set; }
        //public string? credit_type { get; set; }
        public decimal? freecredit_used { get; set; }
        public decimal? paidcredit_used { get; set; }
        public decimal? credit_amount { get; set; }
        public string? credit_currency { get; set; }
        public string? action_type { get; set; }
        public decimal? balance_credit { get; set; }
        public string? employee_id { get; set; }
        public string? transactiondoneby { get; set; }
        public string? transaction_date { get; set; }
        public decimal? order_amount { get; set; }
        public string? payment_provider { get; set; }
        public string? payment_status { get; set; }
        public string? payer_name { get; set; }
        public string? payer_email { get; set; }
        public string? payer_id { get; set; }
        public string? order_comments { get; set; }
        public string? invoice_url { get; set; }
        public string? invoice_name { get; set; }
    }
}
