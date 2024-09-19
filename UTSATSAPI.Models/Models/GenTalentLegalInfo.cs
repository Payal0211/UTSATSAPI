using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentLegalInfo
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string? AgreementStatus { get; set; }
        public int? LastUpdatedbyId { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public bool? IsLifeTimeValid { get; set; }
        public string? EngagementId { get; set; }
        public long? HiringRequestId { get; set; }
        public string? LegalDescription { get; set; }
    }
}
