using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTeamDistribution
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FlagType { get; set; } = null!;
        public long Geoid { get; set; }
        public int SortNo { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
