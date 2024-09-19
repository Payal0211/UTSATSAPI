using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class PostAcceptanceViewModel
    {
        public long TalentID { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Result> postAcceptanceDetail { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Availability_Result> postAcceptanceDetailAvailability { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> postAcceptanceDetailHowSoon { get; set; }
        public bool? IsClientNotificationSent { get; set; }
        public long ContactID { get; set; }
        public string HR_Number { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }
        public string TalentStatus { get; set; }
        public string TalentName { get; set; }
        public bool? IsPriority { get; set; } = false;
        public bool? IsNextWeekPriority { get; set; } = false;
    }
}
