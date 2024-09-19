using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenManageNotificationEmailsHistory
    {
        public long Id { get; set; }
        public long HistoryId { get; set; }
        public string Subject { get; set; } = null!;
        public int RemainderDays { get; set; }
        public DateTime CreatedByDateTime { get; set; }
    }
}
