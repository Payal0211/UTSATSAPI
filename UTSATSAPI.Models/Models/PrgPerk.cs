using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPerk
    {
        public long Id { get; set; }
        public string? Perks { get; set; }
        public bool? IsActive { get; set; }
    }
}
