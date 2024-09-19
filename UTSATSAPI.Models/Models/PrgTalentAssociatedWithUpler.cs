using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentAssociatedWithUpler
    {
        public int Id { get; set; }
        public string? AssociatedWithUplers { get; set; }
        public string? DurationType { get; set; }
        public int? Duration { get; set; }
        public bool? IsActive { get; set; }
    }
}
