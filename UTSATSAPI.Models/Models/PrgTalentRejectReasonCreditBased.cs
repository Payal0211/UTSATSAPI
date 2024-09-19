using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentRejectReasonCreditBased
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? TalentStatusId { get; set; }
    }
}
