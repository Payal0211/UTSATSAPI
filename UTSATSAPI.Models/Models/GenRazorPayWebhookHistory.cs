using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenRazorPayWebhookHistory
    {
        public long Id { get; set; }
        public long? PaymentId { get; set; }
        public string? WebhookResponse { get; set; }
        public string? RazorPayPaymentId { get; set; }
        public decimal? AmountCaptured { get; set; }
        public string? WebhookActionStatus { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
