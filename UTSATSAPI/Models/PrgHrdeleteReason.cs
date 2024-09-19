using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgHrdeleteReason
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
        public int? SequenceId { get; set; }
    }
}
