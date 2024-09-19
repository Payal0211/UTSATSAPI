using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_InterviewerDetails_Result
    {
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public string? InterviewerDesignation { get; set; }
        public string? TypeofInterviewer { get; set; }
        public string? InterviewerEmailID { get; set; }
    }
}
