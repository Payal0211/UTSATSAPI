using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentCategory
    {
        public int Id { get; set; }
        public string? TalentCategory { get; set; }
        public bool? IsActive { get; set; }
    }
}
