using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenIncUserTargetsHistory
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public decimal? UserTarget { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public decimal? SelfTarget { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
