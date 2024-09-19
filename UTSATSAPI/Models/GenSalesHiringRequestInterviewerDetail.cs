using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSalesHiringRequestInterviewerDetail
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public int? TypeofInterviewerId { get; set; }
        public string? InterviewerDesignation { get; set; }
        public string? InterviewerEmailId { get; set; }
        public bool? IsUsedAddMore { get; set; }
        public int? InterviewerExpInMonth { get; set; }
    }
}
