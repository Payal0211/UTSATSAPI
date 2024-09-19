using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.ComplexTypes
{
    [Keyless]
    public class sproc_GetDataOpenPositionForTalent_Result
    {
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDomain { get; set; }
        public Nullable<int> TeamSize { get; set; }
        public string Location { get; set; }
        public string RolesResponsibilities { get; set; }
        public string Generic_Info_About_Company { get; set; }
        public string Requirement { get; set; }
        public string OurObservation { get; set; }
        public string Skills { get; set; }
        public string Cost { get; set; }
        public string HR_Number { get; set; }
        public string AfterHR_TalentStatus { get; set; }
        public string TalentRole { get; set; }
        public Nullable<long> HiringRequestID { get; set; }
        public Nullable<long> HiringRequestDetailID { get; set; }
        public long Shortlisted_InterviewID { get; set; }

    }
}
