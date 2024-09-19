using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentSecondarySkillDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? SecondarySkillId { get; set; }
        public string? TempSkillId { get; set; }
        public decimal? SkillRating { get; set; }
    }
}
