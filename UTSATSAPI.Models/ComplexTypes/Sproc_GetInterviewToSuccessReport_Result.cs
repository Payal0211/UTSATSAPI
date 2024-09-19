using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public  class Sproc_GetInterviewToSuccessReport_Result
    {
        public int ROWID { get; set; }
        public string? TeamID { get; set; }
        public string? I2SLabel { get; set; }
        public long I2SCount { get; set; }
        public string? TeamName { get; set; }
        public decimal I2SPercent { get; set; }
    }
}
