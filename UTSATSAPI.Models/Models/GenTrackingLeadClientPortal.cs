using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTrackingLeadClientPortal
    {
        public long Id { get; set; }
        public string? UtmSource { get; set; }
        public string? UtmMedium { get; set; }
        public string? UtmCampaign { get; set; }
        public string? UtmContent { get; set; }
        public string? UtmTerm { get; set; }
        public string? UtmPlacement { get; set; }
        public string? RefUrl { get; set; }
        public bool? IsFinalSignUp { get; set; }
        public bool? IsJobPost { get; set; }
        public long? CookieId { get; set; }
        public long? ContactId { get; set; }
        public long? Hrid { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Browser { get; set; }
        public string? Device { get; set; }
        public string? IpAddress { get; set; }
    }
}
