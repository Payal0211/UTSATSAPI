using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPayrollType
    {
        public long Id { get; set; }
        public string PayrollType { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
