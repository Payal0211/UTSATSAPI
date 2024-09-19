using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentProjectDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectDetails { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Skills { get; set; }
        public bool? IsCurrentlyWorking { get; set; }
    }
}
