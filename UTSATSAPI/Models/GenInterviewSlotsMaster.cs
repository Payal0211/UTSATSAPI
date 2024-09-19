using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenInterviewSlotsMaster
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public string? Guid { get; set; }
        public int? InterviewStatusId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public long? RescheduledInterviewId { get; set; }
        public string? RescheduledBy { get; set; }
        public int? RescheduledTypeId { get; set; }
        public string? RescheduledReasons { get; set; }
        public string? RescheduledMessageShown { get; set; }
        public int? InterviewRoundCount { get; set; }
        public string? InterviewRoundStr { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
