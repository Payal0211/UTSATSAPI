using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentSelectedInterviewerDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public long? TalentSelectedInterviewId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public long? NextRoundId { get; set; }
        public long? InterviewerId { get; set; }
        public long? InterviewMasterId { get; set; }
    }
}
