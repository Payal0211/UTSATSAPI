using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_BusinessDashboard_Result
    {
        public long? ClientFeedback { get; set; }
        public string? LastFeedbackDate { get; set; }
        public long? OnBoardID { get; set; }
        public string? EngagementId_HRID { get; set; }
        public string? TalentName { get; set; }
        public string? Company { get; set; }
        public string? CurrentStatus { get; set; }
        public string? TSCName { get; set; }
        public string? Geo { get; set; }
        public string? Position { get; set; }
        public int? IsLost { get; set; }
        public string? OldTalent { get; set; }
        public string? ReplacementEng { get; set; }
        public int? NoticePeriod { get; set; }
        public string? KickOff { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
        public string? ContractStartDate { get; set; }
        public string? ContractEndDate { get; set; }
        public string? ActualEndDate { get; set; }
        public decimal? NR { get; set; }
        public decimal? DP_Percentage { get; set; }
        public string? RenewalstartDate { get; set; }
        public string? RenewalendDate { get; set; }
        public int? EngagementTenure { get; set; }
        public string? SOWSignedDate { get; set; }
        public string? NBDName { get; set; }
        public string? AMName { get; set; }
        public string? InvoicingDetails { get; set; }
        public string? InvoiceSentDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceStatus { get; set; }
        public string? DateofPayment { get; set; }
        public string? CreatedByDatetime { get; set; }
        public string? TypeOfHR { get; set; }
        public long? TalentID { get; set; }
        public int? GEOID { get; set; }
        public long? NBDID { get; set; }
        public long? AMID { get; set; }
        public long? CompanyID { get; set; }
        public int TotalRecords { get; set; }
        public long? HR_ID { get; set; }
        public int? TalentLegal_StatusID { get; set; }
        public int? ClientLegal_StatusID { get; set; }
        public int? Kickoff_StatusID { get; set; }
        public int? IsContractCompleted { get; set; }
        public int? IsHRManaged { get; set; }
        public long? ContactTalentPriorityID { get; set; }
        public string? HRNumber { get; set; }
        public long? HR_DetailID { get; set; }
        public int? ClientOnBoarding_StatusID { get; set; }
        public string? FeedbackType { get; set; }
        public decimal? ActiveEngagement { get; set; }
        public int? FeedbcakReceive { get; set; }
        public decimal? AvgNR { get; set; }
        public decimal? AvgDP { get; set; }
        public string? BillRateCurrency { get; set; }
        public string? ClientName { get; set; }
        public string? DeployedSource { get; set; }
        public decimal? ActualBillRate { get; set; }
        public decimal? ActualPayRate { get; set; }
        public decimal? ActualNR { get; set; }
        public int? IsRenewalAvailable { get; set; }
        public string? H_Availability { get; set; }
        public decimal? EngagementCount { get; set; }
        public int? S_TotalDP { get; set; }
        public string? KickOffStatus { get; set; }
        public decimal? DPAmount { get; set; }
        public int? Status_ID { get; set; }
        public string? PayRateCurrency { get; set; }
        public int? IsRenewalContract { get; set; }
        public decimal? Payout_BillRate { get; set; }
        public decimal? Payout_PayRate { get;set; }  
        public int? ReplacementID { get;set; }
        public int? OnBoardLostReasonId { get; set; }
        public string? OnBoardLostReason { get; set; }
        public bool? IsOngoing { get; set; }
    }
}
