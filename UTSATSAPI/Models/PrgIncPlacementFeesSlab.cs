using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgIncPlacementFeesSlab
    {
        public int Id { get; set; }
        public string? PlacementFeesSlab { get; set; }
        public decimal? MinLimitPercentage { get; set; }
        public decimal? MaxLimitPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
