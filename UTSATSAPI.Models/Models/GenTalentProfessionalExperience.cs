using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentProfessionalExperience
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? ProfessionalExperience { get; set; }
        public string? Designation { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Skills { get; set; }
        public bool? IsCurrentlyWorking { get; set; }
    }
}
