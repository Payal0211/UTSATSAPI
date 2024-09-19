using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetEmailDetailForTSCAssignment_Result
    {
        public string? HRID { get; set; }
        public string? EngagementID { get; set; }
        public string? OldTSCName { get; set; }
        public string? OldTSCEmail { get; set; }
        public string? NewTSCName { get; set; }
        public string? NewTSCEmail { get; set; }
        public string? Role { get; set; }
        public string? ClientName { get; set; }
    }
}
