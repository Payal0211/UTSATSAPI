using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Hierarchy_For_Email_Result
    {
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public long? UserId { get; set; }
        public string? ContactNumber { get; set; }
    }
}
