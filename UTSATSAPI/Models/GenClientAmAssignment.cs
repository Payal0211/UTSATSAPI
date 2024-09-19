using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenClientAmAssignment
    {
        public long Id { get; set; }
        public string ClientEmailId { get; set; } = null!;
        public long AssignedAmuserId { get; set; }
        public long InvoiceId { get; set; }
        public long SalesPersonId { get; set; }
        public long GeoId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string? BauUtsTagging { get; set; }
        public string? HrNumber { get; set; }
        public string? EngagemenId { get; set; }
        public long? CompanyId { get; set; }
        public long? EngagementModelId { get; set; }
        public string? TalentName { get; set; }
        public bool? IsImport { get; set; }
    }
}
