using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenAmAssignmentRuleHistory
    {
        public long Id { get; set; }
        public long AmassignmentRuleId { get; set; }
        public string AmassignmentRuleEngagementModelId { get; set; } = null!;
        public long Geoid { get; set; }
        public string BauUtcTagging { get; set; } = null!;
        public long AmuserId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public int Priority { get; set; }
        public long? InvoiceId { get; set; }
        public long? InvoiceHistoryId { get; set; }
        public string InvoiceGuid { get; set; } = null!;
        public bool? IsCreatedFromUts { get; set; }
        public string? HrNumber { get; set; }
        public string? EngagemenId { get; set; }
        public long? CompanyId { get; set; }
        public long? TeamDistributionId { get; set; }
    }
}
