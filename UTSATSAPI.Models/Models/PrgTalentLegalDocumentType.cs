using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentLegalDocumentType
    {
        public int Id { get; set; }
        public string? TalentDocumentType { get; set; }
        public bool? IsValidLifeTime { get; set; }
        public bool? IsEngagementType { get; set; }
        public string? LegalDescription { get; set; }
    }
}
