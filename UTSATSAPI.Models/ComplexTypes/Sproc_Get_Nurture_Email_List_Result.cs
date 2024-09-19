using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Nurture_Email_List_Result
    {
        public long? ClientID { get; set; }                // bigint
        public long? CompanyId { get; set; }               // bigint
        public DateTime? CreatedDateTime { get; set; }     // datetime
        public long? HRID { get; set; }                    // bigint
        public int? TotalJobsPosted { get; set; }          // int
        public int? TotalJobsForProfileShared { get; set; }// int
        public int? TotalJobsExpired { get; set; }         // int
        public int? TotalJobsExpired15Days { get; set; }   // int
        public int? TotalJobsRenewal7Days { get; set; }    // int
        public int? TotalApplicationsReceived { get; set; }// int
        public int? TotalCandidatesinAssessment { get; set; }// int
        public int? TotalApplicationswithVideoScreening { get; set; }// int
        public int? TotalApplicationswithVideoResume { get; set; } // int
        public int? InterviewsScheduled { get; set; }      // int
        public int? TotalCandidatesmovedinInterview { get; set; } // int
        public string? ApplicantWithHighestJobScore { get; set; }  // nvarchar
        public int? AssessmentSharedCount { get; set; }    // int
        public int? ApplicantsNotReviewdPast15Days { get; set; }   // int
        public int? Minimummatchscorenonreviewedcandidates { get; set; } // int
        public int? MONTH { get; set; }                    // int
        public int? YEAR { get; set; }                     // int
    }
}
