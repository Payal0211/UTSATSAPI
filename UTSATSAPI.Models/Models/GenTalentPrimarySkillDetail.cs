using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentPrimarySkillDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? PrimarySkillId { get; set; }
        public string? TempSkillId { get; set; }
        public decimal? YearsOfExp { get; set; }
    }
}
