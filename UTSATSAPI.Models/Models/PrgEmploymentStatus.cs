using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgEmploymentStatus
    {
        public int Id { get; set; }
        public string? EmploymentStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
