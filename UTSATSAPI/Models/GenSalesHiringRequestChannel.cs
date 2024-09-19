using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSalesHiringRequestChannel
    {
        public long Id { get; set; }
        public long HiringRequestId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int MemberCount { get; set; }
        public string ChannelInviteLink { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsSnoozed { get; set; }
        public DateTime? LastMessageDateTime { get; set; }
    }
}
