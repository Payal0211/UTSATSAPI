using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgCountryRegion
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? CountryRegion { get; set; }
        public bool? IsActive { get; set; }
    }
}
