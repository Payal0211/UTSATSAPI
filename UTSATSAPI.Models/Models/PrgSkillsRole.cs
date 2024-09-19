using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSkillsRole
    {
        public long Id { get; set; }
        public long? SkillId { get; set; }
        public int? RoleId { get; set; }
        public int? RankId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
