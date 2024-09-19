using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSlaEditHistory
    {
        public long Id { get; set; }
        public long? HistoryId { get; set; }
        public long? ReasonId { get; set; }
        public string? OtherReason { get; set; }
        public DateTime? Sladate { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
