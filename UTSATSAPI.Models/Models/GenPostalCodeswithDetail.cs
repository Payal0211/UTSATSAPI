using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPostalCodeswithDetail
    {
        public long Id { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
        public string? StateEn { get; set; }
        public string? Province { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
