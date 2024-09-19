using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class UsrUserPriorityCountLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int? OldPriorityCount { get; set; }
        public int? NewPriorityCount { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
    }
}
