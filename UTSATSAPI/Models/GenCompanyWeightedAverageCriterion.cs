using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenCompanyWeightedAverageCriterion
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public int? TargetGroupId { get; set; }
        public int? TeamSizeId { get; set; }
        public int? CurrentOpenPositionId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? LeadPlanId { get; set; }
        public int? ExpansionPlanId { get; set; }
    }
}
