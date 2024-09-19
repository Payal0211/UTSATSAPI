using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardClientContractDetail
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public string? ContractType { get; set; }
        public int? ContractDuration { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int? TotalDurationInMonths { get; set; }
        public string? PunchStartTime { get; set; }
        public string? PunchEndTime { get; set; }
        public string? WorkingDays { get; set; }
        public string? TalentWorkingTimeZone { get; set; }
        public decimal? TalentMonthlyCost { get; set; }
        public string? Currency { get; set; }
        public DateTime? FirstClientBillingDate { get; set; }
        public int? NetPaymentDays { get; set; }
        public int? ContractRenewalSlot { get; set; }
        public DateTime? TalentOnBoardDate { get; set; }
        public TimeSpan? TalentOnBoardTime { get; set; }
        public string? DevicesPoliciesOption { get; set; }
        public string? TalentDeviceDetails { get; set; }
        public decimal? AdditionalCostPerMonthRdpsecurity { get; set; }
        public decimal? DeviceCostAsPerPolicy { get; set; }
        public bool? IsRecurring { get; set; }
        public string? ExpectationFromTalentFirstWeek { get; set; }
        public string? ExpectationFromTalentFirstMonth { get; set; }
        public string? ProceedWithUplersExitPolicyOption { get; set; }
        public string? ProceedWithUplersLeavePolicyOption { get; set; }
        public string? ProceedWithClientLeavePolicyOption { get; set; }
        public string? ProceedWithClientLeavePolicyLink { get; set; }
        public string? ProceedWithClientLeavePolicyFileUpload { get; set; }
        public string? ClientRemark { get; set; }
        public string? TalentRemark { get; set; }
        public bool? IsTalentRaiseConcern { get; set; }
        public string? TalentWorkingTimeZoneConcern { get; set; }
        public string? TalentContractConcern { get; set; }
        public string? TalentDevicePolicesConcern { get; set; }
        public string? TalentLeavePolicyConcern { get; set; }
        public string? TalentExitPolicyConcern { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? TimezonePreferenceId { get; set; }
        public decimal? ClientBaseCost { get; set; }
        public DateTime? UpdatedContractEndDate { get; set; }
        public string? ContractEndDateUpdateReason { get; set; }
        public string? UpdateReasonSupportFilename { get; set; }
        public decimal? TotalApprovedLeaves { get; set; }
        public decimal? TotalNumberofLeaves { get; set; }
        public bool? IsContractCompleted { get; set; }
        public DateTime? LastWorkingDate { get; set; }
        public bool? IsLost { get; set; }
        public DateTime? LostDate { get; set; }
    }
}
