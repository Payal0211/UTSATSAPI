using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyLegalInfo
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentUrl { get; set; }
        public string? DocumentFile { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string? AgreementStatus { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? LegalDescription { get; set; }
    }
}
