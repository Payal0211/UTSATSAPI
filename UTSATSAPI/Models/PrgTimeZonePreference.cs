using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTimeZonePreference
    {
        public int Id { get; set; }
        public int? HrTimeZoneId { get; set; }
        public string? WorkingTimePreference { get; set; }
    }
}
