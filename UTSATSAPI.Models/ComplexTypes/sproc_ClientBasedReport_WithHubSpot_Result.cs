using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_ClientBasedReport_WithHubSpot_Result
    {
        public string StageName { get; set; }
        public int StageValue { get; set; }
    }
}
