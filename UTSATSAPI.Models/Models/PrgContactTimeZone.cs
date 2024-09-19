using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgContactTimeZone
    {
        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public string? TimeZoneTitle { get; set; }
        public string? Description { get; set; }
        public string? TimeZone { get; set; }
        public bool? IsActive { get; set; }
        public int? IsttimeDiffMin { get; set; }
        public string? ShortName { get; set; }
    }
}
