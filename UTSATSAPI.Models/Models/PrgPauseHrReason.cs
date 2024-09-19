using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPauseHrReason
    {
        public long Id { get; set; }
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
    }
}
