using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class HubSpotEventType
    {
        public long Id { get; set; }
        public string? SubscriptionTypeCode { get; set; }
        public string? SubscriptionType { get; set; }
        public DateTime? CraetedDateTime { get; set; }
    }
}
