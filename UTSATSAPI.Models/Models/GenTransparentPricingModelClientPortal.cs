using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTransparentPricingModelClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public long? ContactId { get; set; }
        public int? HiringTypePricingId { get; set; }
        public int? SpecificMonth { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollPartnerName { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public decimal? PricingPercentage { get; set; }
        public bool? Istransparent { get; set; }
    }
}
