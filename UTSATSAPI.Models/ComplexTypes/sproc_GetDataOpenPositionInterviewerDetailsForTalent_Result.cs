using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.ComplexTypes
{
    [Keyless]
    public class sproc_GetDataOpenPositionInterviewerDetailsForTalent_Result
    {
        public string NameOfInterviewer { get; set; }
        public string YearsofExperience { get; set; }
        public string TypeofInterviewer { get; set; }
        public string InterviewerDesignation { get; set; }
        public string LinkedInProfile { get; set; }
        public long HiringRequest_ID { get; set; }
        public long HiringRequest_Detail_ID { get; set; }
        public long Shortlisted_InterviewID { get; set; }
    }
}
