using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenClientCalendarEvent
    {
        public int EventId { get; set; }
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public long SlotGivenById { get; set; }
        public string? Guid { get; set; }
        public bool? IsSlotGivenByClient { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
    }
}
