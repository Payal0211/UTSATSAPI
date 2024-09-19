using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentSelectionHistory
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? UserId { get; set; }
        public int? OldStatusId { get; set; }
        public int? NewStatusId { get; set; }
        public string? Remark { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
