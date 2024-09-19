using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentAssesmentSkillScore
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public long? AssesmentScoreId { get; set; }
        public int? AssesmentSkillId { get; set; }
        public decimal? SkillScore { get; set; }
        public string? AssesmentType { get; set; }
    }
}
