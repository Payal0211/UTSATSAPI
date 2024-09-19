using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result
    {
        public long? CompanyID { get; set; }
        public string? Company { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactName { get; set; }
        public long? ContactID { get; set; }
    }
}
