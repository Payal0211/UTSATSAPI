using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenHubSpotContact
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? CompanyId { get; set; }
        public string? Company { get; set; }
        public int? PortalId { get; set; }
        public long? HubspotOwnerId { get; set; }
        public long? CanonicalVid { get; set; }
        public long? Associatedcompanyid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? Website { get; set; }
        public string? ContactNo { get; set; }
        public bool? IsPrimary { get; set; }
        public string? MergedVids { get; set; }
        public bool? IsContact { get; set; }
        public string? Venture { get; set; }
        public string? Regions { get; set; }
        public string? City { get; set; }
        public string? Skype { get; set; }
        public string? Jobtitle { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsCompanyInserted { get; set; }
        public DateTime? LastInvoiceDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? EngagementModelId { get; set; }
        public long? LastInvoiceId { get; set; }
        public int? GeoId { get; set; }
        public string? ZohoCustomerId { get; set; }
        public long? ZohoOrganizationId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCreatedFromUts { get; set; }
        public string? LeadSource { get; set; }
        public string? Designation { get; set; }
        public string? Lifecyclestage { get; set; }
        public string? HsPersona { get; set; }
    }
}
