using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class sp_Upchat_GetInActiveHRList_Result
    {
        public long? HiringRequestID { get; set; }
    }
}
