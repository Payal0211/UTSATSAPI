using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetCloseHRHistory_Result
    {
        public long HiringRequest_ID { get; set; }
        public long CreatedByID { get; set; }
        public string? FullName { get; set; }
        public string? CreatedByDateTime { get; set; }
        public string? UserRole { get; set; }
    }
}
