using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_SchedularUpdates_Result
    {
        public string? SchedularName { get; set; }
        public string? SPName { get; set; }
        public DateTime? LastRunDateTime { get; set; }
    }
}
