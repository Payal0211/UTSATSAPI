using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetHubSpotCompanyList_Result
    {
        public long? ID { get; set; }
        public long? CompanyID { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
        public string? domain { get; set; }
        public string? CreatedByDatetime { get; set; }
        public int TotalRecords { get; set; }
    }
}
