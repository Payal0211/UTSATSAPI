using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentRole
    {
        public int Id { get; set; }
        public string? TalentRole { get; set; }
        public string? FrontIconImage { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdhoc { get; set; }
        public long? PitchMeRoleId { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? CreatedbyDatetime { get; set; }
        public DateTime? ModifyByDatetime { get; set; }
    }
}
