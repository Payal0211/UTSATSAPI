using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHiringRequestSkillDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public int? SkillId { get; set; }
        public string? Proficiency { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? TempSkillId { get; set; }
    }
}
