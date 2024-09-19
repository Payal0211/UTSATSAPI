using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgRequirement
    {
        public long Id { get; set; }
        public string ReqText { get; set; } = null!;
    }
}
