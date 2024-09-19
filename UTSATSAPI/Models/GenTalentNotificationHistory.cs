using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentNotificationHistory
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public bool? NotificationFlag { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
