using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_FetchInterviewerDetails_Result
    {
        public long SelectedInterviewID { get; set; }
        public long InterviewerID { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public string? InterviewerEmailID { get; set; }
        public decimal yearsOfexp { get; set; }
        public int TypeofInterviewer_ID { get; set; }
        public string? InterviewerDesignation { get; set; }
        public bool IsUsedAddMore { get; set; }
    }
}
