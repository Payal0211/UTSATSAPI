using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAutoCompleteHubSpotCompanies_Result
    {
        public long? Row_ID { get; set; }
        public long? CompanyID { get; set; }
        public string? Company { get; set; }
    }
}
