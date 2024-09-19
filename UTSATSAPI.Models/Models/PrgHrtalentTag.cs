using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHrtalentTag
    {
        public int Id { get; set; }
        public string? TalentTagName { get; set; }
        public bool? IsActive { get; set; }
    }
}
