using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result
    {       
        public long? ATSTalentID { get; set; }
        public string? TalentName { get; set; }
        public long? UTSTalentID { get; set; }
    }
}
