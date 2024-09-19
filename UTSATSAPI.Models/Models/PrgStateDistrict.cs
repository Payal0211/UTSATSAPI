using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgStateDistrict
    {
        public long Id { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Country { get; set; }
        public bool? Status { get; set; }
        public string? RevisedCity { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
    }
}
