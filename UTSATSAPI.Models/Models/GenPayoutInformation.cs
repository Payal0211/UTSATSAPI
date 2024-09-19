using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPayoutInformation
    {
        public long Id { get; set; }
        public long ContactId { get; set; }
        public long TalentId { get; set; }
        public long HiringRequestId { get; set; }
        public long HiringRequestDetailId { get; set; }
        public long OnBoardId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public DateTime? PayOutDate { get; set; }
        public int? TotalDaysinMonth { get; set; }
        public decimal? ClientProRataAmount { get; set; }
        public decimal? ClientPayOutAmount { get; set; }
        public decimal? ClientFinalPayOutAmount { get; set; }
        public string? ClientInvoiceDocument { get; set; }
        public decimal? TalentProRataAmount { get; set; }
        public decimal? TalentProRataAmountInr { get; set; }
        public decimal? PayOutAmount { get; set; }
        public decimal? PaymentInInr { get; set; }
        public decimal? TalentFinalPayOutInInr { get; set; }
        public decimal? Tds { get; set; }
        public decimal? TdscalculatedAmount { get; set; }
        public decimal? PaymentReleasedInr { get; set; }
        public DateTime? ActualPayOutDate { get; set; }
        public int? ClientPayOutStatus { get; set; }
        public int? PayOutStatus { get; set; }
        public decimal? CurrencyExchangeRate { get; set; }
        public DateTime? CurrencyExcahangeDate { get; set; }
        public string? CurrencyExchangeCode { get; set; }
        public string? InvoiceDocument { get; set; }
        public string? UserInvoiceNumber { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public decimal? TotalNumberofLeaves { get; set; }
        public decimal? TotalApprovedLeaves { get; set; }
        public bool? IsProRataMonth { get; set; }
        public decimal? PerDayClientCost { get; set; }
        public decimal? PerDayTalentCostInr { get; set; }
        public DateTime? ClientInvoiceDate { get; set; }
        public int? StatusId { get; set; }
        public long? EsalesInvoiceId { get; set; }
        public string? EsalesInvoiceNumber { get; set; }
        public DateTime? EsalesInvoiceDate { get; set; }
        public decimal? EsalesInvoiceAmount { get; set; }
        public decimal? EsalesInvoiceLineItemTotal { get; set; }
        public string? Guid { get; set; }
        public decimal? TalentCost { get; set; }
        public decimal? ActualBillRate { get; set; }
        public decimal? ActualPayRate { get; set; }
        public decimal? ActualNrPercentage { get; set; }
        public string? ActualCurrency { get; set; }
        public string? ActualCommentsBr { get; set; }
        public string? ActualCommentsPr { get; set; }
        public string? ReasonBr { get; set; }
        public string? ReasonPr { get; set; }
        public string? TalentCurrencyCode { get; set; }
        public decimal? TalentCurrencyExchangeRate { get; set; }
        public int? TscPersonId { get; set; }
        public string? TsceditReason { get; set; }
        public int? AmSalesPersonId { get; set; }
        public string? ContractType { get; set; }
        public int? EngagementStatusId { get; set; }
    }
}
