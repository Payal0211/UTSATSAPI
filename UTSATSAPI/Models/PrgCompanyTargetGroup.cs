using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgCompanyTargetGroup
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Weighted { get; set; }
        public decimal? WeightedPercentage { get; set; }
        public bool IsActive { get; set; }
    }
}
