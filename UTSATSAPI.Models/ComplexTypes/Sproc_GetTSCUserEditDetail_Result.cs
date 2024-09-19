using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetTSCUserEditDetail_Result
    {
        public long? ID { get; set; }
        public string? EngagementID { get; set; }
        public string? TalentName { get; set; }
        public string? CurrentTSCName { get; set; }
        public string? EditTSCReason { get; set; }
        public int? TSCPersonID { get; set; }
    }
}