using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenJobPostSubscriptionHistoryClientPortal
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public string? BalanceType { get; set; }
        public int? CreditBalance { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public DateTime? Sladate { get; set; }
        public long? CompanyId { get; set; }
        public long? PackageId { get; set; }
        public long? PaymentHistoryId { get; set; }
        public bool? IsAdvancePurchase { get; set; }
        public bool? IsUsed { get; set; }
        public int? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
    }
}
