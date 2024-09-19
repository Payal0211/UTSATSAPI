using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSkill
    {
        public long Id { get; set; }
        public string? Skill { get; set; }
        public int? PrimarySkillId { get; set; }
        public int? SecondarySkillId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsActive { get; set; }
    }
}
