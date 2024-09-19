using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgJobStatusClientPortal
    {
        public int Id { get; set; }
        public string JobStatus { get; set; } = null!;
        public bool IsActive { get; set; }
        public string DisplayName { get; set; } = null!;
    }
}
