using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentSelectedPostAcceptanceDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? TalentId { get; set; }
        public long? TalentSelectedInterviewAcceptanceId { get; set; }
        public int? HrTimeZoneId { get; set; }
        public string? TimezonePreference { get; set; }
        public int? TalentPreferableworkingId { get; set; }
        public int? TalentTimeZoneId { get; set; }
        public int? AfterMatchFinalTimeZoneId { get; set; }
        public int? NotAcceptedTimeZoneId { get; set; }
        public bool? IsTimeZoneMatch { get; set; }
        public string? HrAvailability { get; set; }
        public int? HrPartialEngagementTypeId { get; set; }
        public int? HrNoofHoursworking { get; set; }
        public string? TalentAvailability { get; set; }
        public int? TalentPartialEngagementTypeId { get; set; }
        public int? TalentNoofHoursworking { get; set; }
        public bool? IsAvailabilityMatch { get; set; }
        public string? HrHowSoon { get; set; }
        public string? TalentJoining { get; set; }
        public bool? IsJoinningMatch { get; set; }
    }
}
