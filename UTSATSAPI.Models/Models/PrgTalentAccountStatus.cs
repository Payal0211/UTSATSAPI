using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentAccountStatus
    {
        public int Id { get; set; }
        public string? AccountStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
