using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentRole
    {
        public int Id { get; set; }
        public string? TalentRole { get; set; }
        public string? FrontIconImage { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdhoc { get; set; }
    }
}
