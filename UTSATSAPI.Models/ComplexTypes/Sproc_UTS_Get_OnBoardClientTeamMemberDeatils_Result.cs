using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result
    {
        public string? TeamName { get; set; }
        public string? Designation { get; set; }
        public string? Email { get; set; }
        public string? Linkedin { get; set; }
        public string? ReportingTo { get; set; }
        public string? Buddy { get; set; }
    }
}
