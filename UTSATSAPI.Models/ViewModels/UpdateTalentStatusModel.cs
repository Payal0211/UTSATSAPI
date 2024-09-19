using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateTalentStatusModel
    {
        public long HRID { get; set; }
        public long HRDetailID { get; set; }
        public long TalentID { get; set; }
        public int TalentStatusID { get; set; }
        public string? TalentStatus { get; set; }
        public int? RejectReasonID { get; set; }
        public int? OnHoldReasonID { get; set; }
        public int? CancelReasonID { get; set; }
        public string? OtherReason { get; set; }
        public string? Remark { get; set; }
        public long ContactTalentPriorityID { get; set; }


    }
}
