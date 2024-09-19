using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetTalentDetailHRLostPopUp_Result
    {
        public string? TalentName { get; set; }
        public string? TalentEmail { get; set; }
        public string? TalentStatus { get; set; }
    }
}
