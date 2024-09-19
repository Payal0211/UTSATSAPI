using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenJobPostEmploymentDetailsClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public string? EmploymentType { get; set; }
        public string? HowSoon { get; set; }
        public int? WorkingModeId { get; set; }
        public string? CompanyLocation { get; set; }
        public long? AchievementId { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? ClientIpaddress { get; set; }
        public string? Ipaddress { get; set; }
        public long? ContactId { get; set; }
        public string? ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public int? TimezonePreferenceId { get; set; }
        public string? TimeZoneFromTime { get; set; }
        public string? TimeZoneEndTime { get; set; }
        public bool? IsHrTypeDp { get; set; }
        public int? TimeZoneId { get; set; }
        public int? JobTypeId { get; set; }
    }
}
