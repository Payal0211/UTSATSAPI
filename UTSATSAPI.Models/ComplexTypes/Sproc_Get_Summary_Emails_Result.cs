using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Summary_Emails_Result
    {
        public string? subject { get; set; }
        public string? Body { get; set; }
        public string? ToEmailIds { get; set; }
        public string? ToEmailNames { get; set; }
        public string? CCEmailIds { get; set; }
        public string? CCEmailNames { get; set; }
        public string? POCName { get; set; }
        public string? POCEmail { get; set; }
    }
}
