using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAwsSesEmailTracking
    {
        public string? MessageId { get; set; }
        public string? EventType { get; set; }
        public string? ClientDestination { get; set; }
        public string? EmailSubject { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public string? EmailLink { get; set; }
        public string? EmailLinkTags { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? BounceType { get; set; }
        public string? BounceSubType { get; set; }
        public string? BouncedRecipients { get; set; }
        public long? SubjectId { get; set; }
    }
}
