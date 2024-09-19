using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgNbdfunnelStage
    {
        public long Id { get; set; }
        public string StageName { get; set; } = null!;
        public int? Sequence { get; set; }
        public string? ActionId { get; set; }
    }
}
