using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenChannelHistory
    {
        public long Id { get; set; }
        public string? ChannelId { get; set; }
        public long ChannelActionId { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UserEmpId { get; set; }
        public bool? IsAtsuser { get; set; }
        public long? HiringRequestId { get; set; }
    }
}
