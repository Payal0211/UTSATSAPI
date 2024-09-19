using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class OnBoardUpdate
    {
        public Sproc_Get_OnBoardContract_Details_Result onboardDetails { get; set; }
        public Int64 OnboardID { get; set; }
        public List<SelectListItem> drpBuddy { get; set; }

        public List<SelectListItem> drpReportingTo { get; set; }
        public List<GenOnBoardClientDevicePolicyDetail> devicepolicies { get; set; }
        public List<GenOnBoardClientLeavePolicy> leavepolicies { get; set; }
        public GenOnBoardClientTeam onBoardClientTeam { get; set; }

        public Int32 NetPaymentDays { get; set; }

        public Int32 AutoRenewContract { get; set; }

        public int? ContractDutation { get; set; }
        //public DateTime TalentOnBoardingTime { get; set; }
        public string TalentOnBoardingDate { get; set; }
        public string TalentOnBoardingTime { get; set; }
        public string TalentBringOwnRemark { get; set; }

        public string ExpectationFirstWeek { get; set; }
        public string ExpectationFirstMonth { get; set; }

        public string ExpectationAdditionalInformation { get; set; }

        public string LeavePolicyFileName { get; set; }
        public string LeavePolicyPasteLinkName { get; set; }
        public string ExitPolicyFileName { get; set; }
        public string ExitPolicyPasteLinkName { get; set; }

        public string contractStartDate { get; set; }
        public string contractEndDate { get; set; }
        public string  AMUser { get; set; }
        public string BDUser { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
        public decimal? TalentPayRate { get; set; }
        public string? TalentCurrencyCode { get; set; } //Represents the currency code of the talent.
        public GenOnBoardClientLeavePolicy OnBoardClientLeavePolicies { get; set; }
        public string YourLeaveDate { get; set; }
        public List<SelectListItem> drpDay { get; set; }

        public List<DeviceOptionVM> Devicemulticheckbox { get; set; }

        public List<GenOnBoardClientLeavePolicy> OnBoardClientLeavePolicy { get; set; }

        public List<GenOnBoardClientTeam> listOnBoardClientTeam { get; set; }

        public string hdnRadioDevicesPolicies { get; set; }
        public string hdnDevicemulticheckbox { get; set; }
        public string hdnDevicemulticheckboxCost { get; set; }
        public string hdnRadioLeavePolicies { get; set; }

        public string hdnRadioYourLeavePolicies { get; set; }

        public string hdnRadioExitPolicies { get; set; }

        public List<SelectListItem> drpNetPaymentDays { get; set; }
        public List<SelectListItem> drpContractRenewal { get; set; }
        public List<SelectListItem> drpTalentWorkingTime { get; set; }

        public List<SelectListItem> drpTalentPrefWorkingTime { get; set; }
        public List<SelectListItem> drpStartTime { get; set; }
        public List<SelectListItem> drpEndTime { get; set; }
        public List<SelectListItem> drpStartDay { get; set; }
        public List<SelectListItem> drpEndDay { get; set; }
        public List<SelectListItem> drpStartTimeAMPM { get; set; }
        public List<SelectListItem> drpEndTimeAMPM { get; set; }

        public int TalentWorkingTimeZone { get; set; }
        public int TalentWorkingPrefTimeZone { get; set; }

        public string DevicesPoliciesOption { get; set; }
        public string TalentDeviceDetails { get; set; }

        public string ProceedWithUplers_ExitPolicyOption { get; set; }

        public string ProceedWithUplers_LeavePolicyOption { get; set; }
        public string ProceedWithClient_LeavePolicyOption { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string StartTimeAMPM { get; set; }
        public string EndTimeAMPM { get; set; }
        public string TimeZone { get; set; }

        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string CompnayName { get; set; }
        public string HRID { get; set; }
        public string ContractType { get; set; }
        public string WorkingTimeZone { get; set; }
        public string ViewMode { get; set; }
        public decimal? ContactRenewal { get; set; }

        public List<GetTimezonePref> GetTimezonePreference { get; set; }
        public List<GetSummaryDetails> GetSummary { get; set; }
        public string PageName { get; set; }
    }

    public class DeviceOptionVM
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public int Qty { get; set; }
        public decimal? Cost { get; set; }
        public string ID { get; set; }
        public string OtherDetails { get; set; }
        public string DeviceDescription { get; set; }
    }

    public class GetTimezonePref
    {
        public long OnboardID { get; set; }
        public List<PrgTimeZonePreference> TimezonePref { get; set; }
        public List<SelectListItem> drpTalentPrefWorkingTimedata { get; set; }
        public int? TimeZonePrefId { get; set; }
        public long TimeZoneIdValue { get; set; }

    }

    public class GetSummaryDetails
    {
        public long OnboardId { get; set; }
        public List<Sproc_Get_OnBoardSummary_Result> onBoardSummary_Results { get; set; }
        public string PageName { get; set; }
        public Int64 HiringId { get; set; }
    }

}
