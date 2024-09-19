using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgAtshrstatus
    {
        public long Id { get; set; }
        public string? Atshrstatus { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDisplay { get; set; }
        public string? DisplayName { get; set; }
    }
}
