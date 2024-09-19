using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenManageNotificationEmailsHistoryForMonday
    {
        public long Id { get; set; }
        public long HistoryId { get; set; }
        public string Subject { get; set; } = null!;
        public DateTime CreatedByDateTime { get; set; }
    }
}
