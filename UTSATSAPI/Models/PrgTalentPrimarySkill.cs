using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentPrimarySkill
    {
        public int Id { get; set; }
        public string? TalentPrimarySkill { get; set; }
        public bool? IsActive { get; set; }
    }
}
