using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentCategory
    {
        public int Id { get; set; }
        public string? TalentCategory { get; set; }
        public bool? IsActive { get; set; }
    }
}
