using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAwsSesEmailSendingDetail
    {
        public string? MessageId { get; set; }
        public string? EventType { get; set; }
        public string? ClientDestination { get; set; }
        public string? EmailSubject { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
