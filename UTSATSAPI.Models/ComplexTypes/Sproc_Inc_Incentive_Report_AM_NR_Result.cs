using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Inc_Incentive_Report_AM_NR_Result
    {
        public long Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public decimal TeamTarget { get; set; }
        public int TeamAchivedTarget { get; set; }
        public decimal SelfTarget { get; set; }
        public int SelfAchivedTarget { get; set; }
        public decimal TeamPercentange { get; set; }
        public decimal SelfPercentage { get; set; }
    }
}
