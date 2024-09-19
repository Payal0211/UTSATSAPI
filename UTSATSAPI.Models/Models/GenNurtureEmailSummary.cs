using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenNurtureEmailSummary
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public long? CompanyId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? Hrid { get; set; }
        public int? TotalJobsPosted { get; set; }
        public int? TotalJobsForProfileShared { get; set; }
        public int? TotalJobsExpired { get; set; }
        public int? TotalJobsExpired15Days { get; set; }
        public int? TotalJobsRenewal7Days { get; set; }
        public int? TotalApplicationsReceived { get; set; }
        public int? TotalCandidatesinAssessment { get; set; }
        public int? TotalApplicationswithVideoScreening { get; set; }
        public int? TotalApplicationswithVideoResume { get; set; }
        public int? InterviewsScheduled { get; set; }
        public int? TotalCandidatesmovedinInterview { get; set; }
        public string? ApplicantWithHighestJobScore { get; set; }
        public int? AssessmentSharedCount { get; set; }
        public int? ApplicantsNotReviewdPast15Days { get; set; }
        public int? Minimummatchscorenonreviewedcandidates { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
