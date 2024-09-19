using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentAccountStatus
    {
        public int Id { get; set; }
        public string? AccountStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
