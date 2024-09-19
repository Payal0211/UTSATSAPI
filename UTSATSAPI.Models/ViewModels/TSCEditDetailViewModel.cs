using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class TSCEditDetailViewModel
    {
        public List<SelectListItem>? DrpTSCUserList { get; set; }
        public string? EngagementID { get; set; }
        public string? TalentName { get; set; }
        public string? CurrentTSCName { get; set; }
        public string? EditTSCReason { get; set; }
        public int? TSCPersonID { get; set; }
    }
    public class UpdateTSCNameViewModel
    {
        public long? OnBoardID { get; set; }
        public long? TSCUserId { get; set; }
        public string? TSCEditReason { get; set; }
        public long? OldTSC_PersonID { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

}
