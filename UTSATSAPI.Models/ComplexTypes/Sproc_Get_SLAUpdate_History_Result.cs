using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_SLAUpdate_History_Result
    {
        public long SlaHistoryId { get; set; }
        public long HistoryId { get; set; }
        public DateTime? SLADate { get; set; }
        public DateTime? PrevSLADate { get; set; }
        public long? ReasonId { get; set; }
        public string? Reason { get; set; }
        public string? OtherReason { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
