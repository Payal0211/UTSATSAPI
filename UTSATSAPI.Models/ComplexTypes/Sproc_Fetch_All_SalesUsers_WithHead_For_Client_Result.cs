using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Fetch_All_SalesUsers_WithHead_For_Client_Result
    {
        public long ClientID { get; set; }
        public long HRID { get; set; }
        public long SalesUserID { get; set; }
        public long SalesUserHeadID { get; set; }
        public string SalesUserEmail { get; set; }
        public string SalesUserHeadEmail { get; set; }
    }
}
