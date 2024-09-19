using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_OnBoardContract_Details_Result

    {
        public long? HR_ID { get; set; }
        public long? ContactID { get; set; }
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? Client { get; set; }
        public int? NoOfEmployee { get; set; }
        public string? HRNumber { get; set; }
        public string? HRType { get; set; }
        public string? GEO { get; set; }
        public string? Client_POC_Name { get; set; }
        public string? Client_POC_Email { get; set; }
        public string? Industry { get; set; }
        public string? Discovery_Link { get; set; }
        public string? InterView_Link { get; set; }
        public string? JobDescription { get; set; }
        public string? DealSource { get; set; }
        public string? Deal_Owner { get; set; }
        public string? InBoundType { get; set; }
        public int? IsExistClient { get; set; }
        public string? AM_Name { get; set; }
        public int? Payment_NetTerm { get; set; }
        public string? BillRate { get; set; }
        public decimal? FinalHrCost { get; set; }
        public string? PayRate { get; set; }
        public decimal? TalentCost { get; set; }
        public string? UTS_HRAcceptedBy { get; set; }
        public decimal? NRPercentage { get; set; }
        public string? TalentRole { get; set; }
        public string? WorkForceManagement { get; set; }
        public string? TalentName { get; set; }
        public string? TalentEmail { get; set; }
        public string? TalentProfileLink { get; set; }
        public string? Availability { get; set; }
        public string? Talent_Designation { get; set; }
        public long? OnBoardID { get; set; }
        public long? TalentID { get; set; }
        public string? Talent_CurrencyCode { get; set; }
        public string? CurrencySign { get; set; }
        public string? ShiftStartTime { get; set; }
        public string? ShiftEndTime { get; set; }
        public string? EngagemenID { get; set; }
        public decimal? DPAmount { get; set; }
        public decimal? DP_Percentage { get; set; }
        public string? CurrentCTC { get; set; }
        public string? ExpectedSalary { get; set; }
        public bool? IsHRTypeDP { get; set; }
        public string? InVoiceRaiseTo { get; set; }
        public string? InVoiceRaiseToEmail { get; set; }
        public int? UTSContractDuration { get; set; }
        public string? BDR_MDR_Name { get; set; }
        public string? Company_Description { get; set; }
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
        public string? Feedback_Process { get; set; }
        public string? Device_Radio_Option { get; set; }
        public int? DeviceID { get; set; }
        public decimal? TotalCost { get; set; }
        public string? Client_DeviceDescription { get; set; }
        public string? SOWDocument { get; set; }
        public DateTime? ClientLegalDate { get; set; }
        public DateTime? MSASignDate { get; set; }
        public DateTime? TalentLegalDate { get; set; }
        public int? ContractDuration { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? KickOffTimeZone { get; set; }
        public string? TalentReportingPOC { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string? ZohoInvoiceNumber { get; set; }
        public string? IsRenewalInitiated { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? LastWorkingDate { get; set; }
        public DateTime? ClientClosureDate { get; set; }
        public string? UplersfeesAmount { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
    }
}
