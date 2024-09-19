using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTimeZoneCostPreference
    {
        public int Id { get; set; }
        public int? HrTimeZoneId { get; set; }
        public int? CostPreference { get; set; }
        public int? UsfullPreference { get; set; }
        public int? UspartialPreference { get; set; }
        public int? Ukpreference { get; set; }
        public int? Istpreference { get; set; }
        public int? AupartialPreference { get; set; }
        public int? AufullPreference { get; set; }
    }
}
