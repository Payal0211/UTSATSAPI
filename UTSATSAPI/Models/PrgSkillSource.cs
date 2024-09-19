using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgSkillSource
    {
        public int Id { get; set; }
        public string? SkillSource { get; set; }
        public bool? IsActive { get; set; }
    }
}
