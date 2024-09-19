using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result
    {
        public long HRID { get; set; }
        public string? HRNumber { get; set; }
        public int StatusID { get; set; }
        public string? HRStatus { get; set; }
        public int NoofTalents { get; set; }
    }
}
