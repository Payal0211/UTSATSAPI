using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyHistory
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? LinkedInProfile { get; set; }
        public int? CompanySize { get; set; }
        public int? CurrencyId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Industry { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public bool? IsActive { get; set; }
        public string? Location { get; set; }
        public int? ClientStatusId { get; set; }
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
        public bool? IsFromSignUp { get; set; }
        public decimal? JpcreditBalance { get; set; }
        public int? CompanyTypeId { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public int? AnotherCompanyTypeId { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public string? Action { get; set; }
        public bool? IsVettedProfile { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
        public bool? IsGspaceCreated { get; set; }
        public string? GspaceId { get; set; }
        public string? GspaceTokenObject { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? HiringTypePricingId { get; set; }
    }
}
