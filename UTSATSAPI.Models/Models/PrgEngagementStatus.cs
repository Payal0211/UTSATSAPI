using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgEngagementStatus
    {
        public int Id { get; set; }
        public string? EngagementStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
