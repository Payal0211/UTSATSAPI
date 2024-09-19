using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Check_Validation_Message_For_Close_HR_Result
    {
        public string? message { get; set; }
        public string? NameEmail { get; set; }
        public string? btnmessage { get; set; }
    }
}
