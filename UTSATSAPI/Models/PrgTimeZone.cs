using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTimeZone
    {
        public int Id { get; set; }
        public string? TimeZoneTitle { get; set; }
        public int? TalentPreferableWorkingId { get; set; }
        public int? HrTimeZoneId { get; set; }
        public bool? IsActive { get; set; }
        public int? RateCardCostPreference { get; set; }
    }
}
