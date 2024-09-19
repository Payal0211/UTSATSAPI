using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenPostAcceptanceDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? TalentId { get; set; }
        public long? ShortListedId { get; set; }
        public string? TimeZoneIds { get; set; }
        public string? StrTimeZone { get; set; }
        public string? AcceptedAvailability { get; set; }
        public string? AcceptedHowsoon { get; set; }
        public string? RadioShift { get; set; }
        public string? RadioAvailability { get; set; }
        public string? RadioHowsoon { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
