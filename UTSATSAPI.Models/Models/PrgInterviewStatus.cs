using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgInterviewStatus
    {
        public int Id { get; set; }
        public string? InterviewStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
