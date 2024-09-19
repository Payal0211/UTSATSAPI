using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
   [Keyless]
    public class sp_UTS_GetPOCUserIDByCompanyID_Edit_Result
    {
        public long? POCUserID { get; set; }
        public string? POCName { get; set; }

        public long? HRID { get; set; }
        public string? Sales_AM_NBD { get; set; }
    }
}
