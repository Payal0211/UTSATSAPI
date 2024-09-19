using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenHrAtstalentDetailsCtp
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? CtpId { get; set; }
        public long? AtsTalentId { get; set; }
        public string? TalentEmailId { get; set; }
        public string? TalentName { get; set; }
        public string? TalentDesignation { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? NoticePeriod { get; set; }
        public string? AgreedShift { get; set; }
        public string? PreferredAvailability { get; set; }
        public string? HrType { get; set; }
        public string? HrpricingType { get; set; }
        public string? HrtypeText { get; set; }
        public string? TalentExpectedFeeYearly { get; set; }
        public string? TalentCurrentFeeYearly { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
