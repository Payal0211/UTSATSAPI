using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentSkillBenchMark
    {
        public int Id { get; set; }
        public string? TalentSkill { get; set; }
        public int? BenchMark { get; set; }
    }
}
