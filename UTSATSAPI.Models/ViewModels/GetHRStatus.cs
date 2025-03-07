using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class GetHRStatus
    {
        public long HR_ID { get; set; }
        public string HR_Status { get; set; } = "";
        public string HR_Sub_Status { get; set; } = "";
        public string ActionDoneBy { get; set; } = "";
        public string ActionDate { get; set; } = "";
    }
}
