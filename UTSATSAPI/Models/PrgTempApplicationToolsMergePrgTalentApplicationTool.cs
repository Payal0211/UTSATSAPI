using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTempApplicationToolsMergePrgTalentApplicationTool
    {
        public long Id { get; set; }
        public long? TempApplicationToolsId { get; set; }
        public long? PrgApplicationToolsId { get; set; }
        public int? MergeBy { get; set; }
        public DateTime? MergeDatetime { get; set; }
    }
}
