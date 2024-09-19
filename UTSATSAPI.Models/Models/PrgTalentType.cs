using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentType
    {
        public int Id { get; set; }
        public string? TalentType { get; set; }
        public bool? IsRemoveValidation { get; set; }
    }
}
