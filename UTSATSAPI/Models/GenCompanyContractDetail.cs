using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenCompanyContractDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? SigningAuthorityFirstName { get; set; }
        public string? SigningAuthorityLastName { get; set; }
        public string? SigningAuthorityName { get; set; }
        public string? SigningAuthorityEmail { get; set; }
        public string? SigningAuthorityDesignation { get; set; }
        public string? LegalCompanyName { get; set; }
        public string? LegalCompanyAddress { get; set; }
        public bool? IsAcceptPolicy { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
