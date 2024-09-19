using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHiringRequestStatus
    {
        public int Id { get; set; }
        public string? HiringRequestStatus { get; set; }
        public bool? IsActive { get; set; }
        public string? Color { get; set; }
        public int? JobStatusId { get; set; }
        public bool? IsUsedinAts { get; set; }
        public string? AtsHiringRequestStatus { get; set; }
    }
}
