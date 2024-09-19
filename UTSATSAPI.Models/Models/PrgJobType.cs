using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgJobType
    {
        public int Id { get; set; }
        public string? JobType { get; set; }
        public bool? IsHiringLimited { get; set; }
        public bool? IsActive { get; set; }
    }
}
