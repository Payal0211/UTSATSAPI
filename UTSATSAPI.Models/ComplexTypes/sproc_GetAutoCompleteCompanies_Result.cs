using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAutoCompleteCompanies_Result
    {
        public long? Row_ID { get; set; }
        public long? CompanyID { get; set; }
        public long? ContactID { get; set; }
        public string? Company { get; set; }
        public string? EmailID { get; set; }
        public string? Client { get; set; }

    }
}
