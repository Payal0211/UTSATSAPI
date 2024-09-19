using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_SalesUserWithHead_FOr_HiringRequest_Result
    {
        public long ClientID { get; set; }
        public long HRID { get; set; }
        public long SalesUserID { get; set; }
        public long SalesUserHeadID { get; set; }
        public string SalesUserEmail { get; set; }
        public string SalesUserHeadEmail { get; set; }
        public string GSpaceID { get; set; }
        public string TokenObject { get; set; }
        public string ClientEmail { get; set; }
    }
}
