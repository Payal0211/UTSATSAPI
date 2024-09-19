using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentStatusAfterClientSelection
    {
        public int Id { get; set; }
        public string? TalentStatus { get; set; }
        public bool? IsActive { get; set; }
        public string? Color { get; set; }
        public int? OrderSequence { get; set; }
        public bool? IsDisplay { get; set; }
    }
}
