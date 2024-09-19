using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentSelectedNextRoundInterviewDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public long? TalentSelectedInterviewId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public int? InterviewRound { get; set; }
        public string? NextRoundOption { get; set; }
        public string? SlotGiven { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public bool? IsLaterSlotGiven { get; set; }
    }
}
