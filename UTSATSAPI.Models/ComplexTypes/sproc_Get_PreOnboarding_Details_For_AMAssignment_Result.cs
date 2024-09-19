using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_PreOnboarding_Details_For_AMAssignment_Result
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
        public long? AMUserID { get; set; }
        public string? Payment_NetTerm { get; set; }
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
        public int? PayementNetTerm { get; set; }
        public long? OnBoardID { get; set; }
        public long? TalentID { get; set; }
        public string? Talent_CurrencyCode { get; set; }
        public string? CurrencySign { get; set; }
        public string? ShiftStartTime { get; set; }
        public string? ShiftEndTime { get; set; }
        public string? EngagemenID { get; set; }
        public decimal? DPAmount { get; set; }
        public decimal? DP_Percentage { get; set; }
        public decimal? UplersFeesAmount { get; set; }
        public decimal? CurrentCTC { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public bool? IsHRTypeDP { get; set; }
        public string? ISTShiftStartTime { get; set; }
        public string? ISTShiftEndTime { get; set; }
        public string? ModeOfWork { get; set; } //from gen_OnBoardTalents
        public int? StateID { get; set; }       //from gen_OnBoardTalents
        public string? StateName { get; set; }  //from gen_OnBoardTalents
        public string? CityName { get; set; }   //from gen_OnBoardTalents
        public int? TSCUserId { get; set; }    //from gen_OnBoardTalents
        public string? PageTitle { get; set; }
    }
}
