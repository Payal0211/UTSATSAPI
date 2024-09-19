using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_ValidateAddClient_Result
    {
        public int? status { get; set; }
        public string? result { get; set; }
        public long? companyID { get; set; }

    }
}
