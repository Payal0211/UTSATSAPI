using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentKeyQuality
    {
        public int Id { get; set; }
        public string? KeyQualities { get; set; }
        public bool? IsActive { get; set; }
    }
}
