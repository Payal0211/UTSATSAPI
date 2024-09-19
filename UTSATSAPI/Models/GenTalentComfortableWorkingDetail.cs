using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentComfortableWorkingDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? ComfortableWorkingId { get; set; }
    }
}
