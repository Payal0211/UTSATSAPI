using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_OnBoardDetailFor_C2H_Result
    {
        public decimal? DPNRPercentage { get; set; }    
        public int? IsConvertToHireApplicable { get; set; }

        public string? Currency { get; set; }
    }
}
