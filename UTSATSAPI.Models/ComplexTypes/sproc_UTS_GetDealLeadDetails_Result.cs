using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class sproc_UTS_GetDealLeadDetails_Result
    {
        public long Deal { get; set; }
        public string LeadSource { get; set; }
        public string PipeLine { get; set; }
        public string BDR { get; set; }
        public string SalesConsulant { get; set; }
    }
}
