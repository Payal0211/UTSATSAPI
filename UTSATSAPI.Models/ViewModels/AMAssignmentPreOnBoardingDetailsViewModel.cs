using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class AMAssignmentPreOnBoardingDetailsViewModel
    {

        public sproc_Get_PreOnboarding_Details_For_AMAssignment_Result? PreOnboardingDetailsForAMAssignment { get; set; }
        public List<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result>? CurrentHRs { get; set; }

        public List<SelectListItem>? DrpTSCUserList { get; set; }
        public List<SelectListItem>? DRPNetPaymentDays { get; set; }
        public List<SelectListItem>? DRPLeadTypes { get; set; }
        public List<SelectListItem>? DRPLeadUsers { get; set; }
        public List<SelectListItem>? DRPStartTime { get; set; }
        public List<SelectListItem>? DRPEndTime { get; set; }

        //CTA
        public DynamicOnBoardCTA? dynamicOnBoardCTA { get; set; }

        /// <summary>
        /// Ensures if the update action will Assign AM or not.
        /// </summary>
        public bool? AssignAM { get; set; }

        public List<SelectListItem>? DRPHRAcceptedByUserList { get; set; }

        #region Second Tabs Details
        public sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result? SecondTabAMAssignmentOnBoardingDetails { get; set; }
        public List<PrgOnBoardPolicyDeviceMaster>? deviceMaster { get; set; }
        public List<SelectListItem>? bindLeavePolicyDrp { get; set; }
        public string? Exit_Policy { get; set; }
        public string? Feedback_Process { get; set; }
        public string? UplersLeavePolicy { get; set; }
        public List<GenOnBoardClientTeam>? onBoardClientTeam { get; set; }

        #endregion

        public bool? IsFirstTabReadOnly { get; set; } = false;
        public bool? IsSecondTabReadOnly { get; set; } = false;
        public bool? IsTransparentPricing { get; set; } = false; // HR level data.

        // UTS-7389: If replacement is Yes then get replacement details
        public GenOnBoardTalentsReplacementDetail? ReplacementDetail { get; set; }

        public List<MastersResponseModel> ReplacementEngAndHR { get; set; } = new List<MastersResponseModel>();
    }

    public class UpdatePreOnBoardingDetailsForAMAssignment
    {
        public long? HR_ID { get; set; }
        public long? CompanyID { get; set; }
        public string? Deal_Owner { get; set; }
        public string? Deal_Source { get; set; }
        public string? Lead_Type { get; set; }
        public string? Industry_Type { get; set; }
        public long? Onboard_ID { get; set; }
        public string? EngagemenID { get; set; }
        public bool? AssignAM { get; set; }
        public long? TalentID { get; set; }
        public string? TalentShiftStartTime { get; set; }
        public string? TalentShiftEndTime { get; set; }
        public decimal? PayRate { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? UplersFeesAmount { get; set; }
        public decimal? UplersFeesPerc { get; set; }
        public int? NetPaymentDays { get; set; }
        public decimal? NRMargin { get; set; }
        public int? ModeOFWorkingID { get; set; }
        public string? City {  get; set; }
        public int? StateID { get; set; }
        public string? Talent_Designation { get; set; }
        public int? AMSalesPersonID { get; set; }
        public long? TSCUserId { get; set; }

        // UTS-7389: Get the replacement details from UI.
        public bool? IsReplacement { get; set; }        
        public TalentReplacement talentReplacement { get; set; } = new TalentReplacement();
        public UpdatePreOnBoardingDetailsForSecondTabAMAssignment updateClientOnBoardingDetails { get; set; }
    }

    public class DynamicOnBoardCTA
    {
        public CTAInfo? AMAssignment { get; set; }
        public CTAInfo? GotoOnboard { get; set; }
        public CTAInfo? TSCAssignment { get; set; }
    }
    public class UpdatePreOnBoardingDetailsForSecondTabAMAssignment
    {
        public long? HR_ID { get; set; }
        public long? CompanyID { get; set; }
        public string? SigningAuthorityName { get; set; }
        public string? SigningAuthorityEmail { get; set; }
        public int? ContractDuration { get; set; }
        public long? OnBoardID { get; set; }
        public string? about_Company_desc { get; set; }
        public string? Talent_FirstWeek { get; set; }
        public string? Talent_FirstMonth { get; set; }
        public string? SoftwareToolsRequired { get; set; }
        public string? DevicesPoliciesOption { get; set; }
        public string? TalentDeviceDetails { get; set; }
        public decimal? AdditionalCostPerMonth_RDPSecurity { get; set; }
        public bool? IsRecurring { get; set; }
        public string? ProceedWithUplers_LeavePolicyOption { get; set; }
        public string? ProceedWithClient_LeavePolicyOption { get; set; }
        public string? ProceedWithClient_LeavePolicyLink { get; set; }
        public string? LeavePolicyFileName { get; set; }
        public string? Exit_Policy { get; set; }
        public string? hdnRadioDevicesPolicies { get; set; }
        public string? Device_Radio_Option { get; set; }
        public int? DeviceID { get; set; }
        public string? Client_DeviceDescription { get; set; }
        public decimal? TotalCost { get; set; }
        public string? Radio_LeavePolicies { get; set; }
        public string? LeavePolicyPasteLinkName { get; set; }
        public List<TeamMembers>? TeamMembers { get; set; }

        //UTS-7389: Get the replacement details from UI.
        //public bool? IsReplacement { get; set; }
        //public TalentReplacement talentReplacement { get; set; } = new TalentReplacement();
    }

    public class OnBoardStatusDetails
    {
        public GenOnBoardTalent? GenOnBoardTalent { get;set; }
        public bool? IsThirdTabReadOnly { get; set; } = false;
        public bool? IsFourthTabReadOnly { get; set; } = false;

        // UTS-7389: If replacement is Yes then get replacement details
        public GenOnBoardTalentsReplacementDetail? ReplacementDetail { get; set; }
        public List<MastersResponseModel> ReplacementEngAndHR { get; set; } = new List<MastersResponseModel>();
    }

}