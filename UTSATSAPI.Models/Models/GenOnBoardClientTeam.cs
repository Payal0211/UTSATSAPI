using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenOnBoardClientTeam
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Email { get; set; }
        public string? Linkedin { get; set; }
        public string? ReportingTo { get; set; }
        public string? Buddy { get; set; }
        public bool? IsActive { get; set; }
    }
}
