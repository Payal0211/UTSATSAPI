using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GET_PostAcceptance_detail_HowSoon_Result
    {
        public string Client_JoinWithin { get; set; }
        public string Client_Readytoworkhrs { get; set; }
        public string Client_ReadytoworkShift { get; set; }
        public long ContactID { get; set; }
        public long HiringRequest_Detail_ID { get; set; }
        public long HiringRequest_ID { get; set; }
        public long ID { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<bool> OptionMatch { get; set; }
        public long PrimaryKey { get; set; }
        public string RecommendedMessage { get; set; }
        public long Shortlisted_InterviewID { get; set; }
        public string TableName { get; set; }
        public string Talent_JoinWithin { get; set; }
        public string Talent_Readytoworkhrs { get; set; }
        public string Talent_ReadytoworkShift { get; set; }
        public string TalentRole { get; set; }
    }
}