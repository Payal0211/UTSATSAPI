using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSchedularHistory
    {
        public int Id { get; set; }
        public string? SchedularName { get; set; }
        public string? Spname { get; set; }
        public DateTime? LastRunDateTime { get; set; }
        public string? Spprofiler { get; set; }
    }
}
