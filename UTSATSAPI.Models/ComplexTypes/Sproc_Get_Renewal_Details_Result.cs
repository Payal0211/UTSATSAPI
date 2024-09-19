using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Renewal_Details_Result
    {
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractEndDate { get; set; }
        public decimal BR { get; set; }
        public decimal PR { get; set; }
        public string EngagemenId { get; set; }
        public string ContactName { get; set; }
        public string Company { get; set; }
        public string TalentName { get; set; }
        public decimal NRPercentage { get; set; }
        public string Currency { get; set; }
    }
}
