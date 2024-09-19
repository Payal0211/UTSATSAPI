using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSubscriptionPackageClientPortal
    {
        public long Id { get; set; }
        public string? PackageName { get; set; }
        public decimal? Package { get; set; }
        public int? NoOfJobs { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPayPerViewPlan { get; set; }
    }
}
