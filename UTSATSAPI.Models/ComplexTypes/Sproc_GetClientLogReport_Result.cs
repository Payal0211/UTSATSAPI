using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
        [Keyless]
        public partial class Sproc_GetClientLogReport_Result
        {
            public long CompanyId { get; set; }
            public string? Company { get; set; }
            public string? Email { get; set; }
            public int LoginCount { get; set; }
            public string? LoggedInTime { get; set; }
            public string? LastModifiedDatetime { get; set; }
            public int NoOfTalentHired { get; set; }
            public string? LoginStatus { get; set; }
            public string? ActivityType { get; set; }
            public int TotalRecords { get; set; }
            public long ContactID { get; set; }
        }
}
