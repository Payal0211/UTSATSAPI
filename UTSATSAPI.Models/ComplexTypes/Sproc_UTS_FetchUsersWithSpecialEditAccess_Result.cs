using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_FetchUsersWithSpecialEditAccess_Result
    {
        public bool? AllowSpecialEdit { get; set; }
        public bool? AllowHRDelete { get; set; }
        public bool? AllowHRAddInDemoAccount { get; set; }
    }
}
