using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class HRDetailViewModel
    {
        public int HR_Id { get; set; }
        public sp_get_HRClientDetails_Result? ClientDetail { get; set; }
        //public List<sp_get_HRHistory_Result>? HRHistory { get; set; }
        public List<sp_UTS_get_HRHistory_UsingPagination_Result>? HRHistory { get; set; }
        //public List<sp_get_HRTalentDetails_Result>? HRTalentDetails { get; set; }
        public List<sp_UTS_get_HRTalentDetails_UsingPagination_Result>? HRTalentDetails { get; set; }
        public List<TalentWiseInterviewDetails> InterviewSlotDetails { get; set; }
        public List<DynamicSalaryInfo> DynamicSalaryInfo { get; set; }
        public List<sp_getInterviewdetailsForViewAllHR_Result>? HRInterviewDetails { get; set; }
        public string? AdhocPoolValue { get; set; }
        public sproc_FetchMissingAction_Result? FetchMissingAction { get; set; }
        public List<sproc_NextActionsForTalent_Result>? NextActionsForTalent { get; set; }
        public List<Sproc_Get_TalentDetails_For_Managed_HR_Result>? HRTalentDetailsManagedHR { get; set; }
        public Boolean HRManagedORNot { get; set; }

        public string? JobStatus { get; set; }
        public int? JobStatusID { get; set; }
        public string? HRStatus { get; set; }
        public int? HRStatusID { get; set; }
        public string? HRRoleStatus { get; set; }
        public int? HRRoleStatusID { get; set; }
        
        public bool IsHRClosed { get; set; }
        public int? IsAccepted { get; set; }
        public bool ShowAssignTalent { get; set; }
        public Nullable<int> TR_Accepted { get; set; }
        public Nullable<int> NoofTalents { get; set; }
        public List<SP_NextActionsForManagedHR_Result>? NextActionsForManagedHR { get; set; }
        public string? HRAction { get; set; }
        public long? HRActionID { get; set; }
        public int HRStatusCode { get; set; }
        public int StarMarkedStatusCode { get; set; }
        public List<SelectListItem>? UsersToTag { get; set; }
        public bool? Is_HRTypeDP { get; set; }
        public bool? DpFlag { get; set; }
        
        public string companyCategory { get; set; }
        public bool? IsPartnerShipHR { get; set; }
        public GenSalesHiringRequestDetail? HRDetails { get; set; } //Added a new property to have all the HR related details

        public long? OnBoardID { get; set; }
        public long? ReplaceOnBoardID { get; set; }

        //CTA
        public DynamicCTA? dynamicCTA { get; set; }

        /// <summary>
        /// UTS-3998: Allow edit to specific set of users.
        /// </summary>
        public bool? AllowSpecialEdit { get; set; } = false;

        /// <summary>
        /// Represents the GUID to identify if the HR is created from front-end.
        /// </summary>
        public string? Guid { get; set; }

        /// <summary>
        /// Upchat ChannelID
        /// </summary>
        public string? ChannelID { get; set; }
        public bool IsDirectHR { get; set; }
        public PayPerHireModel transparentModel { get; set; }
        public string? EngagementType { get; set; }
        public string? BudgetTitle { get; set; }
        public string? BudgetText { get; set; }
        public PayPerCreditModel PayPerCreditModel { get; set; }
        public bool? IsPayPerHire { get; set; }
        public bool? IsPayPerCredit { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsVettedProfile { get; set; }

        /// <summary>
        /// UTS-7484: Allow delete to specific set of users.
        /// </summary>
        public bool? AllowHRDelete { get; set; } = false;
    }

    public class DynamicCTA
    {
        public List<CTAInfo> CTA_Set1 { get; set; }
        public List<CTAInfo> CTA_Set2 { get; set; }
        public CTAInfo CloneHR { get; set; }
        public CTAInfo MatchMaking { get; set; }
        public CTAInfo CloseHr { get; set; }
        public CTAInfo? ReopenHR { get; set; }
        public CTAInfo? UpdateTR { get; set; }
        public List<Talent_CTA> talent_CTAs { get; set; }
    }

    public class Talent_CTA
    {
        public long TalentID { get; set; }
        public List<CTAInfo> cTAInfoList { get; set; }
    }

    public class CTAInfo
    {
        public CTAInfo(string? Key, string Label, bool? IsEnabled)
        {
            this.key = Key;
            this.label = Label;
            this.IsEnabled = IsEnabled;
        }
        public string? key { get; set; }
        public string? label { get; set; }
        public bool? IsEnabled { get; set; }
    }

    public class TalentWiseInterviewDetails
    {
        public long? TalentID { get; set; }
        public List<InterviewSlotDetails> SlotList { get; set; }
    }

    public class InterviewSlotDetails
    {
        public string? StrDateTime { get; set; }
        public string? Date { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? TimeZone { get; set; }
    }

    public class DynamicSalaryInfo
    {
        public long? TalentID { get; set; }
        public List<TalentDynamicInfo> TalentDynamicInfo { get; set; }
    }

    public class TalentDynamicInfo
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
        public bool? IsEditable { get; set; }
    }
}
