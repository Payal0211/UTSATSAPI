using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentStatusAfterClientSelection
    {
        public int Id { get; set; }
        public string? TalentStatus { get; set; }
        public bool? IsActive { get; set; }
        public string? Color { get; set; }
        public int? OrderSequence { get; set; }
    }
}
