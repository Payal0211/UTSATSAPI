using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CloneHRViewModel
    {
        [Required]
        public long hrid { get; set; }
        public long? companyId { get; set; }
        public HybridModel? hybridModel { get; set; }
        
    }
    public class HybridModel
    {
        public int? PayPerType { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public bool? IsPostaJob { get; set; } = false;
        public bool? IsProfileView { get; set; } = false;
        public bool? IsVettedProfile { get; set; } = false;
    }

    public class CloneHRViewModelDemoAccount
    {        
        public List<CloneHRListDemoAccount> cloneHRLists { get; set; } = new List<CloneHRListDemoAccount>();
    }

    public class CloneHRListDemoAccount
    {
        public long? companyId { get; set; }
        public long HRID { get; set; }
        public string? HR_Number { get; set; }
    }
}
