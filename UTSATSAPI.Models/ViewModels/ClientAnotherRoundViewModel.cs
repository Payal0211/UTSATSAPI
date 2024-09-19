using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

namespace UTSATSAPI.Models.ViewModels
{
    public class ClientAnotherRoundViewModel
    {
        public int HiringRequestID { get; set; }
        public int HiringDetailID { get; set; }
        public int TalentID { get; set; }
        public int ContactID { get; set; }
        public long ShortlistInterviewerID { get; set; }
        public int? TimeZoneId { get; set; }
        public List<RescheduleSlot>? Timeslot { get; set; }
        public List<InterviewerDetails>? interviewerDetails { get; set; }
        public string AnotherRoundInterviewOption { get; set; } // Yes, Append, Add
        public string AnotherRoundTimeSlotOption { get; set; } // Now, Later
    }

    public class InterviewerDetails
    {
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public string? InterviewerDesignation { get; set; }
        public int? TypeofInterviewer { get; set; }
        public string? InterviewerEmailID { get; set; }
    }
}
