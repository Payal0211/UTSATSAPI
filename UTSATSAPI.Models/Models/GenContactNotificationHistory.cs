using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactNotificationHistory
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public bool? NotificationFlag { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
