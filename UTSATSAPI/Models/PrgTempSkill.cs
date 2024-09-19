using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTempSkill
    {
        public long Id { get; set; }
        public string? TempSkillId { get; set; }
        public string? TempSkill { get; set; }
        public string? Role { get; set; }
        public int? RoleId { get; set; }
        public int? SkillSourceId { get; set; }
        public string? Source { get; set; }
        public int? StatusId { get; set; }
        public bool? AddedByTalent { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? Guid { get; set; }
        public long? PrgSkillId { get; set; }
        public string? RejectReason { get; set; }
        public bool? IsSentInEmail { get; set; }
    }
}
