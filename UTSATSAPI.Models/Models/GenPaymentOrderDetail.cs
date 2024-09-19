using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPaymentOrderDetail
    {
        public long Id { get; set; }
        public string? OrderId { get; set; }
        public string? OrderResponse { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? ReceiptId { get; set; }
        public long? CompanyId { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
    }
}
