using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenIncSalesIncentiveTargetSummaryHistory
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? ParentId { get; set; }
        public long? UserRoleId { get; set; }
        public int? TeamTarget { get; set; }
        public int? SelfTarget { get; set; }
        public decimal? TeamPercentange { get; set; }
        public decimal? SelfPercentage { get; set; }
        public decimal? TeamAmount { get; set; }
        public decimal? SelfAmount { get; set; }
        public int? TotalAchievedTarget { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? ContractBooster { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreateByDateTime { get; set; }
        public decimal? TalentIncentivePercentage { get; set; }
        public decimal? DpPercentage { get; set; }
        public decimal? AmTargetPercentage { get; set; }
        public decimal? AmNrPercentage { get; set; }
    }
}
