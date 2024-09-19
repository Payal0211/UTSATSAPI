using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenContactTalentPriority
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public int? RoleId { get; set; }
        public long? TalentId { get; set; }
        public string? TalentPriority { get; set; }
        public bool? AllowedtoSelectforHr { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? TalentStatusIdBasedOnHr { get; set; }
        public int? PerticularHrTimeZoneIdonAcceptance { get; set; }
        public string? CostAfterAcceptance { get; set; }
        public string? HrCost { get; set; }
        public bool? TalentKeepsOwnPreferences { get; set; }
        public decimal? FinalHrCost { get; set; }
        public decimal? HrBaseCost { get; set; }
        public bool IsAdhoc { get; set; }
        public decimal? CurrencyFinalHrCost { get; set; }
        public decimal? CurrencyHrBaseCost { get; set; }
        public decimal? Nrpercentage { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public long? PreviousTalentStatusIdBasedOnHr { get; set; }
        public int? RejectReasonId { get; set; }
        public string? OtherRejectReason { get; set; }
        public string? OnHoldRemark { get; set; }
        public string? LossRemark { get; set; }
        public decimal? TalentCost { get; set; }
        public decimal? Dpamount { get; set; }
        public DateTime? NrorDpchangeDateTime { get; set; }
        public bool IsHrtypeDp { get; set; }
        public decimal? DpPercentage { get; set; }
        public decimal? CurrentCtc { get; set; }
        public string? TalentCurrencyCode { get; set; }
        public decimal? ExchangeRate { get; set; }
        public bool? IsReplacement { get; set; }
        public int? SalesPersonId { get; set; }
    }
}
