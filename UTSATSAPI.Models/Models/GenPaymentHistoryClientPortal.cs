using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPaymentHistoryClientPortal
    {
        public long Id { get; set; }
        public string? PaymentId { get; set; }
        public string? Guid { get; set; }
        public string? PaymentToken { get; set; }
        public string? PayerId { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? InvoiceId { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? CreatedById { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ReceiptId { get; set; }
        public string? PaymentOrderDetail { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TotalInvoiceAmount { get; set; }
        public decimal? Sgstpercentage { get; set; }
        public decimal? Cgstpercentage { get; set; }
        public decimal? Igstpercentage { get; set; }
        public string? BillingAddress { get; set; }
        public string? RazorPayPaymentStatus { get; set; }
        public long? CompanyId { get; set; }
        public string? OrderId { get; set; }
    }
}
