using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.ComplexTypes
{
    [Keyless]
    public class sproc_FetchHiringInterviewerDetails_Result
    {
        public string InterviewerName { get; set; }
        public string InterviewLinkedin { get; set; }
        public string InterviewerEmailID { get; set; }
        public decimal yearsOfexp { get; set; }
        public int TypeofInterviewer_ID { get; set; }
        public string InterviewerDesignation { get; set; }
        public long ID { get; set; }
    }
}
