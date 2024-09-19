using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenChannelMessageReaction
    {
        public long Id { get; set; }
        public long ChannelId { get; set; }
        public long MessageId { get; set; }
        public long UserId { get; set; }
        public byte[] Emoticon { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
