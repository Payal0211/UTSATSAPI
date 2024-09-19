using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgApitimeZoneResponse
    {
        public int Id { get; set; }
        public string? FromZoneName { get; set; }
        public string? FromAbbreviation { get; set; }
        public string? FromTimeStamp { get; set; }
        public string? ToZoneName { get; set; }
        public string? ToAbbreviation { get; set; }
        public string? ToTimeStamp { get; set; }
        public long? Offset { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? CountryCode { get; set; }
    }
}
