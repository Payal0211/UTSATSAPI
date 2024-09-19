using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentTimeZone
    {
        public int Id { get; set; }
        public string? TalentTimeZone { get; set; }
        public bool? IsActive { get; set; }
        public string? TimeDuration { get; set; }
        public decimal? ClientCostPercentage { get; set; }
        public decimal? TalentCostPercentage { get; set; }
    }
}
