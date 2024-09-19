using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class sproc_UTS_GetDealPrimary_SecondaryClient_Result
    {
        public string Name { get; set; }
        public string EmaiID { get; set; }
        public string Phone { get; set; }
        public string linkedin { get; set; }
    }
}
