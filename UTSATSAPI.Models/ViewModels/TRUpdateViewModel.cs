using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class TRUpdateViewModel
    {
        public TRUpdateViewModel()
        {
            TalentDetails = new();
        }
        public long HRID { get; set; }
        public long HRStatusID { get; set; }
        public string HRStatus { get; set; }
        public decimal TR { get; set; }
        public List<TRUpdateTalentDetail> TalentDetails { get; set; }
    }
    public class TRUpdateTalentDetail
    {
        public long ATS_TalentID { get; set; }
        public string TalentStatus { get; set; }
        public long UTS_TalentID { get; set; }
        public decimal Talent_USDCost { get; set; }
        public string? Reason { get; set; }
        public string? Talent_RejectReason { get; set; }
        public string? RejectedBy { get; set; }


    }
}
