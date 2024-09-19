using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetCompanyWhatsappDetails_Result
    {
        public long? DetailID { get; set; }
        public string? GroupID { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCreatedBy { get; set; }
        public string? GroupCreatedDate { get; set; }
        public string? GroupMember { get; set; }
        public bool? IsAdmin { get; set; }
        public string? EmployeeID { get; set; }
        public string? ContactNumber { get; set; }
    }
}
