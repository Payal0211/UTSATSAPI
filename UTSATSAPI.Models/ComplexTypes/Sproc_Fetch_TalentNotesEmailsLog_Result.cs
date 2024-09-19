using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Fetch_TalentNotesEmailsLog_Result
    {
        public long? HRID { get; set; }
        public string? HR_Number { get; set; }
        public string? RequestForTalent { get; set; }
        public long? ContactID { get; set; }
        public long? SalesUserID { get; set; }
        public string? EmailID { get; set; }
        public string? FullName { get; set; }
        public string? Company { get; set; }
        public long? CompanyID { get; set; }
    }
}
