using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSalesHiringRequestChannelUserDetail
    {
        public long Id { get; set; }
        public long ChannelId { get; set; }
        public long? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserDesignation { get; set; }
    }
}
