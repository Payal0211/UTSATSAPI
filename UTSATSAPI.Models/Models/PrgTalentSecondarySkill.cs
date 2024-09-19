using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentSecondarySkill
    {
        public int Id { get; set; }
        public string? TalentSecondarySkill { get; set; }
        public bool? IsActive { get; set; }
    }
}
