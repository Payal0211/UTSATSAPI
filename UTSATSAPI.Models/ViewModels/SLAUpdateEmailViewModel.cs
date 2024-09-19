using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class SLAUpdateEmailViewModel
    {
        public long HrId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string OldSlaDetails { get; set; } = string.Empty;
        public string UpdatedSlaDetails { get; set; } = string.Empty;
        public string UpdatedSlaReason { get; set; } = string.Empty;
        public List<string> ToEmail { get; set; } = new List<string>();
        public List<string> ToEmailName { get; set; } = new List<string>();
    }
}
