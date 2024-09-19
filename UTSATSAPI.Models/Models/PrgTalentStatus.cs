using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentStatus
    {
        public int Id { get; set; }
        public string? TalentStatus { get; set; }
        public bool? IsUsedtoSetupAccount { get; set; }
        public bool? IsDisplayedinAdmin { get; set; }
        public bool? IsActive { get; set; }
    }
}
