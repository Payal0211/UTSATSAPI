using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgHiringRequestReportStage
    {
        public long Id { get; set; }
        public string HrreportStageName { get; set; } = null!;
        public int Sequence { get; set; }
    }
}
