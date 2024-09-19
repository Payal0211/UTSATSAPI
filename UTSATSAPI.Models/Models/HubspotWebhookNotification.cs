using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class HubspotWebhookNotification
    {
        public long Id { get; set; }
        public long? EventId { get; set; }
        public long? SubscriptionId { get; set; }
        public long? PortalId { get; set; }
        public string? OccurredAt { get; set; }
        public long? EventTypeId { get; set; }
        public string? SubscriptionType { get; set; }
        public int? AttemptNumber { get; set; }
        public long? ObjectId { get; set; }
        public string? ChangeSource { get; set; }
        public string? ChangeFlag { get; set; }
        public long? AppId { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public bool IsProcessed { get; set; }
        public string? ExceptionDetails { get; set; }
    }
}
