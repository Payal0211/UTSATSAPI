using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenIncTargetAcheivedDetail
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? UserRoleId { get; set; }
        public long? ClientId { get; set; }
        public string? CompanyCategory { get; set; }
        public long? HiringRequestId { get; set; }
        public long? OnBoardId { get; set; }
        public long? InvoiceId { get; set; }
        public int? TargetMonth { get; set; }
        public int? TargetYear { get; set; }
        public DateTime? TargetDate { get; set; }
        public int? AcheivedTargetCount { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? SlaMasterName { get; set; }
        public decimal? AmNrPercentage { get; set; }
        public string? AmNrSlab { get; set; }
        public string? CbSlab { get; set; }
        public decimal? CbSlabAmount { get; set; }
        public decimal? CbCalculatedAmount { get; set; }
        public string? TiSlab { get; set; }
        public decimal? TiSlabAmount { get; set; }
        public decimal? TiCalculatedAmount { get; set; }
        public string? DpSlab { get; set; }
        public decimal? DpSlabAmount { get; set; }
        public decimal? DpCalculatedAmount { get; set; }
    }
}
