using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPostJobEmailNotificationClientPortal
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public string? Subject { get; set; }
        public int? ReminderDays { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
