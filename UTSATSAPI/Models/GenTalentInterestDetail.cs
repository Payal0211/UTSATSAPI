using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentInterestDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? InterestId { get; set; }
        public string? TempInterestId { get; set; }
    }
}
