using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentStatusClientPortal
    {
        public int Id { get; set; }
        public string? TalentStatus { get; set; }
        public int? TalentStatusIdBasedOnHr { get; set; }
        public string? ActionName { get; set; }
        public bool? IsActive { get; set; }
    }
}
