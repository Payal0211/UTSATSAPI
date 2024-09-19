using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class TalentReplacement
    {
        public long OnboardId { get; set; }
        public long? ReplacementID { get; set; }
        public long? HiringRequestID { get; set; }
        public long? TalentId { get; set; }
        public DateTime? LastWorkingDay { get; set; }
        public int? LastWorkingDateOption { get; set; }
        public int? Noticeperiod { get; set; }
        public string? ReplacementStage { get; set; }
        public string? ReasonforReplacement { get; set; }
        public string? ReplacementInitiatedby { get; set; }
        public int? ReplacementHandledByID { get; set; }
        public long? EngagementReplacementOnBoardID { get; set; }
        public long? ReplacementTalentId { get; set; }
        public string? EngHRReplacement { get; set; }

    }
    [Keyless]
    public class SaveTalentReplacement
    {
        public long Id { get; set; }
    }
}
