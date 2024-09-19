using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateCancelEngagementViewModel
    {
        public int ContractDetailID { get; set; }
        public DateTime? LastWorkingDate { get; set; }
        public string Reason { get; set; }
        public int? LostReasonID { get; set; }
        public string? EngcancelType { get; set; }

    }
}
