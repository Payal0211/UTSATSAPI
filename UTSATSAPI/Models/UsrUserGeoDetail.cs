using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class UsrUserGeoDetail
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public int? GeoId { get; set; }
    }
}
