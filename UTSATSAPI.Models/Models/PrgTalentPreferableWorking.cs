using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentPreferableWorking
    {
        public int Id { get; set; }
        public string? TalentWorkingHrs { get; set; }
        public string? WorkingZone { get; set; }
        public bool? IsActive { get; set; }
    }
}
