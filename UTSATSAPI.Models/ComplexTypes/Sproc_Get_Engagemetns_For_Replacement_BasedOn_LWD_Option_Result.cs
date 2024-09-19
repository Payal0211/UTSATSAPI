using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option_Result
    {
        public long Value { get; set; }
        public string Text { get; set; }
    }
}
