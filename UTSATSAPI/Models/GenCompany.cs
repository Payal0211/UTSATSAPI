using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenCompany
    {
        public long Id { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public int? DomainId { get; set; }
        public string? LinkedInProfile { get; set; }
        public int? CompanySize { get; set; }
        public int? TimeZoneId { get; set; }
        public int? CurrencyId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Industry { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public bool? IsNdasigned { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public bool? IsActive { get; set; }
        public string? Location { get; set; }
        public int? ClientStatusId { get; set; }
        public int? TeamManagement { get; set; }
        public decimal? Score { get; set; }
        public string? Category { get; set; }
        public int? GeoId { get; set; }
        public int? AmSalesPersonId { get; set; }
        public int? NbdSalesPersonId { get; set; }
        public string? DiscoveryCall { get; set; }
        public string? LeadType { get; set; }
        public string? CompanyLogo { get; set; }
        public long? HubSpotCompany { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public long? ParentCompanyId { get; set; }
        public string? IndustryType { get; set; }
    }
}
