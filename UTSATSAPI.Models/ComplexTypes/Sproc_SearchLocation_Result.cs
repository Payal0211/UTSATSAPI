using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_SearchLocation_Result
    {
        public long? Row_ID { get; set; }
        public string? Location { get; set; }
    }
}
