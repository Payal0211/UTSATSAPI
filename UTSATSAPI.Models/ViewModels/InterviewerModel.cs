using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class InterviewerViewModel
    {
        public InterviewerModel interviewer { get; set; }
        public List<InterviewerModel> MoreInterviewer { get; set; }
    }

    public class InterviewerModel
    {
        public long InterviewMasterID { get; set; }
        public long InterviewerId { get; set; }
        public string? interviewerFullName { get; set; }
        public string? interviewerEmail { get; set; }
        public string? interviewerLinkedin { get; set; }
        public string? interviewerDesignation { get; set; }
        public decimal? yearsOfexp { get; set; }
        public int? TypeofPerson { get; set; }
        public int? SelectedInterviewerID { get; set; }
        public bool IsAdd { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUpdate { get; set; }
    }

}
