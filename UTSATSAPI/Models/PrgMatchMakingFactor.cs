using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgMatchMakingFactor
    {
        public int Id { get; set; }
        public string MatchMakingFactor { get; set; } = null!;
        public decimal Weightage { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
