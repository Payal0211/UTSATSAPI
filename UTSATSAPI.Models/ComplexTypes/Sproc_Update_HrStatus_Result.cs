using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Update_HrStatus_Result
    {
        public long? HR_ID { get; set; }
        public string? HR_Status { get; set; }
        public string? HR_Sub_Status { get; set; }
        public string? ActionDoneBy { get; set; }
        public DateTime? ActionDate { get; set; }
    }
}
