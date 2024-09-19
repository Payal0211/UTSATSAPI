using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSkillSource
    {
        public int Id { get; set; }
        public string? SkillSource { get; set; }
        public bool? IsActive { get; set; }
    }
}
