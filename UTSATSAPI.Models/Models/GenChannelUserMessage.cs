using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenChannelUserMessage
    {
        public long Id { get; set; }
        public long ChannelId { get; set; }
        public long MessageSentFrom { get; set; }
        public bool IsMessageReplied { get; set; }
        public long? MessageRepliedTo { get; set; }
        public long? MessageRepliedId { get; set; }
        public bool IsMessageBookmarked { get; set; }
        public string Message { get; set; } = null!;
        public bool IsSent { get; set; }
        public DateTime MessageSentDateTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsActivity { get; set; }
    }
}
