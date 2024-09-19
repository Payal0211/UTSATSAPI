using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_talent_engagement_Details_For_PHP_API_Result
    {
        public Int64? ID { get; set; }
        public string? EngagemenID { get; set; }
        public long HiringRequest_ID { get; set; }
        public Int64? Talent_ID { get; set; }
        public Int64? ATS_Talent_ID { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? EngagementStatus { get; set; }
        public string? Talent_status { get; set; }
        public DateTime? joining_date { get; set; }
        public DateTime? Lost_date { get; set; }
        public DateTime? Last_working_date { get; set; }
        public string? talent_statustag { get; set; }
        public string? Action { get; set; }
        public DateTime? Action_date { get; set; }


    }
}
