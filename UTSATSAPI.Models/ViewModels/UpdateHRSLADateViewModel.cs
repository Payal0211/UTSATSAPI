using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateHRSLADateViewModel
    {
        public long HrId { get; set; }
        public int ReasonId { get; set; }
        public string OtherReason { get; set; }
        public DateTime SlaRevisedDate { get; set; }
    }
}
