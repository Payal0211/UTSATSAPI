using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenOnBoardClientInvoiceSummary
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public decimal? TalentBaseCost { get; set; }
        public decimal? TalentAdditionalPayout { get; set; }
        public decimal? TalentPackage { get; set; }
        public decimal? DeviceCostAsPerPolicy { get; set; }
        public bool? IsRecurring { get; set; }
        public decimal? TotalFirstMonthCost { get; set; }
        public decimal? NextMonthCost { get; set; }
        public decimal? NextMonthBaseCost { get; set; }
        public decimal? NextMonthAdditionalPayout { get; set; }
        public decimal? NextMonthDeviceCost { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public decimal? TalentPayOut { get; set; }
    }
}
