using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenShortlistedTalentInterviewDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public int? TimeZoneId { get; set; }
        public string? InterviewSlot { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public TimeSpan? InterviewStartTime { get; set; }
        public TimeSpan? InterviewEndTime { get; set; }
        public bool? IsTalentConfirmed { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public DateTime? IstScheduledOn { get; set; }
        public TimeSpan? IstInterviewStartTime { get; set; }
        public TimeSpan? IstInterviewEndTime { get; set; }
        public decimal? DurationInHours { get; set; }
        public long? NextRoundId { get; set; }
        public long? InterviewMasterId { get; set; }
    }
}
