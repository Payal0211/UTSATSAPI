using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTempInterestMergePrgTalentInterest
    {
        public long Id { get; set; }
        public long? TempInterestId { get; set; }
        public long? PrgInterestId { get; set; }
        public int? MergeBy { get; set; }
        public DateTime? MergeDatetime { get; set; }
    }
}
