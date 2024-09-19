using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenHubSpotCompany
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public int? PortalId { get; set; }
        public string? SourceId { get; set; }
        public string? Source { get; set; }
        public long? HubspotOwnerId { get; set; }
        public long? HsObjectId { get; set; }
        public decimal? Annualrevenue { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Domain { get; set; }
        public string? Industry { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public int? Numberofemployees { get; set; }
        public string? LinkedinCompanyPage { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsContactInserted { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public long? LastInvoiceId { get; set; }
        public DateTime? LastInvoiceDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? EngagementModelId { get; set; }
        public int? GeoId { get; set; }
        public string? ZohoPrimaryCustomerId { get; set; }
        public bool? IsContactExist { get; set; }
        public bool? IsWallet { get; set; }
        public decimal? WalletBalance { get; set; }
        public bool? IsCreatedFromUts { get; set; }
        public int? AssignedAmuserId { get; set; }
        public string? Category { get; set; }
        public string? RegionName { get; set; }
        public string? Lifecyclestage { get; set; }
        public string? LeadSourceChannel { get; set; }
    }
}
