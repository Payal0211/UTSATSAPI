using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ClientHiringRequestViewModel
    {
        public Nullable<int> RoleID { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> YearOfExp { get; set; }
        public string Timezone { get; set; }
        public string TimeZone_FromTime { get; set; }
        public string TimeZone_EndTime { get; set; }
        public string DurationType { get; set; }
        public string HowSoon { get; set; }
        public string JDURLParsingContentRoles { get; set; }
        public string JDURLParsingContentSkills { get; set; }
        public string JDURLParsingContentRequirement { get; set; }
        public Nullable<int> NoofTalents { get; set; }
        public string Cost { get; set; }
        public string Availability { get; set; }
        public string Timezone_Preference { get; set; }
        public Nullable<int> Timezone_Preference_ID { get; set; }
        public string FrontIconImage { get; set; }


        public Nullable<int> ContactId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDomain { get; set; }
        public Nullable<int> TeamSize { get; set; }
        public string Location { get; set; }
        public string RolesResponsibilities { get; set; }
        public string Generic_Info_About_Company { get; set; }
        public string Requirement { get; set; }
        public string OurObservation { get; set; }
        public string Skills { get; set; }
        public List<SalesHiringRequestInterviewerDetails> listInterveiwDetails { get; set; }
    }

    public class SalesHiringRequestInterviewerDetails
    {
        public string InterviewerName { get; set; }
        public string InterviewLinkedin { get; set; }
        public Nullable<decimal> InterviewerYearofExperience { get; set; }
        public Nullable<int> TypeofInterviewer_ID { get; set; }
        public string InterviewerDesignation { get; set; }
        public string TypeofInterviewer { get; set; }
        public string InterviewerEmailID { get; set; }
    }
}
