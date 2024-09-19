using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_HR_POC_ClientPortal_Result
    {
        public long? HiringRequestId { get; set; }
        public long? HrwiseContactId { get; set; }
        public string? Guid { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? ContactNo { get; set; }
        public bool? ShowEmailToTalent { get; set; }
        public bool? ShowContactNumberToTalent { get; set; }
        public bool? IsDefaultUser { get; set; }
        public string? IInfoMsg { get; set; }
    }
}
