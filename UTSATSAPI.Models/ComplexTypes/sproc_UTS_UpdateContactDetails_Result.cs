using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_UpdateContactDetails_Result
    {
        public long? ContactID { get; set; }
        public bool? IsNewClient { get; set; }
    }
}
