using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentRejectReason
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public bool? IsDisplay { get; set; }
        public int? TalentStatusId { get; set; }
    }
}
