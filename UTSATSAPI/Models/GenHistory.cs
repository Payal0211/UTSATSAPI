using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenHistory
    {
        public long Id { get; set; }
        public long ActionId { get; set; }
        public long HiringRequestId { get; set; }
        public long TalentId { get; set; }
        public long? ContactTalentPriorityId { get; set; }
        public long? InterviewMasterId { get; set; }
        public long? OnBoardId { get; set; }
        public decimal? TalentCost { get; set; }
        public decimal? HrCost { get; set; }
        public DateTime? HrAcceptedDateTime { get; set; }
        public decimal? HrPercentage { get; set; }
        public bool HistoryFrom { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public DateTime? SlaDueDate { get; set; }
        public bool? IsManagedByClient { get; set; }
        public bool? IsManagedByTalent { get; set; }
        public bool? IsActive { get; set; }
        public int? NextActionId { get; set; }
        public long? SalesUserId { get; set; }
        public long? OpsUserId { get; set; }
        public bool? IsReplacement { get; set; }
        public long? OldSalesUserId { get; set; }
        public int? AppActionDoneBy { get; set; }
        public int? TalentStatusIdBasedOnHr { get; set; }
    }
}
