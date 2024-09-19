using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_HRIDEngagementID_ForReplacement_Result
    {
        public long? ID { get; set; }
        public string? IDvalue { get; set; }
        public string? Dropdowntext { get; set; }
        public bool? IsHR { get; set; }
    }
}
