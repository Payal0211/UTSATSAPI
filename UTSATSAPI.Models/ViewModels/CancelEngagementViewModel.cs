using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CancelEngagementViewModel
    {
        public string LastWorkingDate { get; set; }
        public int ContractDetailID { get; set; }
        public bool IsConvertToHireApplicable { get; set; }
        public decimal DPNRPercentage { get; set; }
        public string Currency { get; set; }
        public string ClosureDate { get; set; }
    }
}
