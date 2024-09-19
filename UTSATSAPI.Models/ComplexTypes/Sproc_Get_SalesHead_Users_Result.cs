using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_SalesHead_Users_Result
    {
        public string? FullName { get; set; }
        public long? ID { get; set; }
    }
}
