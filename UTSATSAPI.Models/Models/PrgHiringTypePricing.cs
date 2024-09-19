using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHiringTypePricing
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTransparent { get; set; }
        public string? EngagementType { get; set; }
        public int? HrtypeId { get; set; }
        public decimal? PricingPercent { get; set; }
    }
}
