using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetActiveSalesUserList_Result
    {
        public long ID { get; set; }
        public string? EmailID { get; set; }
        public string? EmployeeID { get; set; }
        public string? FullName { get; set; }

    }
}
