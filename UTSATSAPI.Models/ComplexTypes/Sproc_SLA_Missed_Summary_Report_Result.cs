using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_SLA_Missed_Summary_Report_Result
    {
        public int Delay_in_HR_acceptance { get; set; }
        public int Delay_in_Profile_share { get; set; }
        public int Delay_in_Profile_Feedback { get; set; }
        public int Delay_in_Interview_Schedule { get; set; }
        public int Delay_in_Interview_Feedback { get; set; }
        public int Delay_in_OnBoarding { get; set; }
    }
}
