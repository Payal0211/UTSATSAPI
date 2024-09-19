using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgJobPostStageMessage
    {
        public long Id { get; set; }
        public long? AtsHrstatusId { get; set; }
        public string? MessageBeforeActionDone { get; set; }
        public string? MessagePostActionDone { get; set; }
        public bool? IsActive { get; set; }
    }
}
