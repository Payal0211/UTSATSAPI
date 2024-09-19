using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentPendingAction
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? PendingAction { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
