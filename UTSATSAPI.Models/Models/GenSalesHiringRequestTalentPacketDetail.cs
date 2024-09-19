using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHiringRequestTalentPacketDetail
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? TalentId { get; set; }
        public string? AssessmentTool { get; set; }
        public string? ClientAssesmentReport { get; set; }
        public string? Score { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public string? TalentResumeLink { get; set; }
    }
}
