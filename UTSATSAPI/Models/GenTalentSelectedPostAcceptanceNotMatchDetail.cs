using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentSelectedPostAcceptanceNotMatchDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? TalentId { get; set; }
        public long? TalentSelectedInterviewAcceptanceId { get; set; }
        public string? TalentTimeZonePreferences { get; set; }
        public string? TalentAvailability { get; set; }
        public string? TalentHowSoon { get; set; }
        public bool? OptionSelected { get; set; }
        public string? RecommendedMessage { get; set; }
    }
}
