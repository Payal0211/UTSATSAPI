using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenOnBoardTalentsReplacementDetail
    {
        public long Id { get; set; }
        public long HiringRequestId { get; set; }
        public long OldTalentId { get; set; }
        public long ContactId { get; set; }
        public long OnboardId { get; set; }
        public DateTime? LastWorkingDay { get; set; }
        public string ReplacementReason { get; set; } = null!;
        public int NoticePeriod { get; set; }
        public DateTime ReplacementDate { get; set; }
        public long? ReplaceTalentId { get; set; }
        public bool IsLost { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public DateTime? ClientAgreementDateForReplacement { get; set; }
        public DateTime? TalentAgreementDateForReplacement { get; set; }
        public long? NewOnBoardId { get; set; }
        public DateTime? OldTalentClosureDate { get; set; }
        public DateTime? ReplaceTalentClosureDate { get; set; }
        public string? ReplacementStage { get; set; }
        public int? LastWorkingDateOption { get; set; }
        public string? ReplacementInitiatedBy { get; set; }
        public int? ReplacementHandledById { get; set; }
        public int? LostMonth { get; set; }
        public int? LostYear { get; set; }
        public long? NewHrid { get; set; }
    }
}
