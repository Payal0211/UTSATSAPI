using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentSecondarySkill
    {
        public int Id { get; set; }
        public string? TalentSecondarySkill { get; set; }
        public bool? IsActive { get; set; }
    }
}
