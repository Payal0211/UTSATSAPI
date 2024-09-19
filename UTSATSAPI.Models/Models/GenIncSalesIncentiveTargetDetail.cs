using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenIncSalesIncentiveTargetDetail
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? UserRoleId { get; set; }
        public long? ClientId { get; set; }
        public long? HrId { get; set; }
        public long? OnBoardId { get; set; }
        public long? InvoiceId { get; set; }
        public long? TalentId { get; set; }
        public int? AssignedSelfTarget { get; set; }
        public int? AchievedSelfTarget { get; set; }
        public decimal? SelfPercentageSlab { get; set; }
        public decimal? SelfAmount { get; set; }
        public decimal? ContractBooster { get; set; }
        public decimal? AmTargetAmount { get; set; }
        public decimal? AmNrAmount { get; set; }
        public decimal? TalentIncentiveAmount { get; set; }
        public decimal? DpAmount { get; set; }
        public string? SlabName { get; set; }
        public int? IncMonth { get; set; }
        public int? IncYear { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public decimal? AmNrPercentage { get; set; }
        public string? AmNrSlab { get; set; }
        public decimal? Amount { get; set; }
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
