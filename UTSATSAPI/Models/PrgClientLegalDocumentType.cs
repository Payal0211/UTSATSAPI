using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgClientLegalDocumentType
    {
        public int Id { get; set; }
        public string? ClientDocumentType { get; set; }
        public bool? IsValidLifeTime { get; set; }
        public bool? IsEngagementType { get; set; }
        public string? LegalDescription { get; set; }
    }
}
