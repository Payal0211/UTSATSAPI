using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPitchMeRole
    {
        public long Id { get; set; }
        public long? PitchMeRoleId { get; set; }
        public string? PitchMeRole { get; set; }
        public DateTime? CreatedDatetime { get; set; }
    }
}
