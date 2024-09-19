using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class UsrUserGeoDetail
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public int? GeoId { get; set; }
    }
}
