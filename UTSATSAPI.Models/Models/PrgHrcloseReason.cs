using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHrcloseReason
    {
        public int Id { get; set; }
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
