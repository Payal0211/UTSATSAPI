using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUtsOnBoardLineItem
    {
        public long Id { get; set; }
        public string UtsGuid { get; set; } = null!;
        public int PrefferedEngagementModelId { get; set; }
        public int InvoiceTypeId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int InvoicePatternId { get; set; }
        public int TypeOfWork { get; set; }
        public int Technology { get; set; }
        public string? ProjectName { get; set; }
        public int? ProjectIdOrderId { get; set; }
        public int? QuoteTypeId { get; set; }
        public int? QuoteTat { get; set; }
        public int? QuoteTatType { get; set; }
        public string? FrequencyPeriod { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Note { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal? Discount { get; set; }
        public decimal SubTotal { get; set; }
        public string RowStatus { get; set; } = null!;
        public bool? IsCreatedFromUts { get; set; }
        public string DedicatedUser { get; set; } = null!;
        public int ManageSelfManage { get; set; }
        public string? Ponumber { get; set; }
        public int ProjectExecutionId { get; set; }
        public string Svid { get; set; } = null!;
        public bool? IsAddContractEndDateInItemDesc { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? TempProjectSelect { get; set; }
        public long? EsalesLineItemId { get; set; }
        public bool? IsLineItemDelete { get; set; }
        public string? EngagemenId { get; set; }
    }
}
