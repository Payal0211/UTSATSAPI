using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Priority_Set_Remaining_Count_Details_Result
    {
        public string FullName { get; set; }
        public int AssignedCount { get; set; }
        public int RemainingCount { get; set; }
    }
}
