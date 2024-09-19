using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTempApplicationTool
    {
        public long Id { get; set; }
        public string? TempApplicationToolsId { get; set; }
        public string? TempApplicationTools { get; set; }
        public string? ImageName { get; set; }
        public int? ApplicationToolsourceId { get; set; }
        public string? Source { get; set; }
        public int? StatusId { get; set; }
        public bool? AddedByTalent { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? Guid { get; set; }
        public long? PrgApplicationToolsId { get; set; }
        public string? RejectReason { get; set; }
        public bool? IsSentInEmail { get; set; }
    }
}
