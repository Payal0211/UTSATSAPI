using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgRejectionMsgForTalent
    {
        public int Id { get; set; }
        public int? RejectionReasonId { get; set; }
        public string? MainKey { get; set; }
        public string? RejectionMessageForTalent { get; set; }
        public bool? IsActive { get; set; }
    }
}
