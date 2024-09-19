using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_UTS_ClientPortalTracking_Details_Result
    {
        public int ActionID { get; set; } 
        public string? Actions { get; set; }
        public int? TotalRecords { get; set; }
    }
}
