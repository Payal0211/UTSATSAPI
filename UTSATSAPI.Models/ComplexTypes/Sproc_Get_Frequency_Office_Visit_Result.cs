using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Frequency_Office_Visit_Result
    {
        public int? ID { get; set; }
        public string? Frequency { get; set; }
    }
}
