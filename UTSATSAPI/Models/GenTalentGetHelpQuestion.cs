using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentGetHelpQuestion
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? GetHelpQuestion { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
