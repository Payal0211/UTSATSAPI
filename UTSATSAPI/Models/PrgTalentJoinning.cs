using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentJoinning
    {
        public int Id { get; set; }
        public string? Joinning { get; set; }
        public bool? IsActive { get; set; }
    }
}
