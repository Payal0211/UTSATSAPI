using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHrStatusDisplay
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? StatusCode { get; set; }
        public string? BackGroundColor { get; set; }
        public string? TextColor { get; set; }
        public bool? IsActive { get; set; }
    }
}
