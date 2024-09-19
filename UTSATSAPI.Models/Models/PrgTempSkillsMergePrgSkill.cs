using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTempSkillsMergePrgSkill
    {
        public long Id { get; set; }
        public long? TempSkillId { get; set; }
        public long? PrgSkillId { get; set; }
        public int? MergeBy { get; set; }
        public DateTime? MergeDatetime { get; set; }
    }
}
