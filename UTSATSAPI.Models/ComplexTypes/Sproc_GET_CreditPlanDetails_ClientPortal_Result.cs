using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GET_CreditPlanDetails_ClientPortal_Result
    {
        public long? ID { get; set; }
        public string? CurrentPlan { get; set; }
        public string? PackageDetail { get; set; }
        public int? UpComingJobsRenewal { get; set; }
        public int? ActiveJobs { get; set; }
        public int? ClosedJobs { get; set; }
        public decimal? CreditsUsed { get; set; }
        public decimal? CreditBalance { get; set; }
        public string? ActionDate { get; set; }
        public string? ActionDescription { get; set; }
        public string? JobRole { get; set; }
        public string? HRStatus { get; set; }
        public string? CreditSpent { get; set; }
        public string? CreditBalanceAfterSpent { get; set; }
        public long CompanyID { get; set; }
        public string? CreditDebit { get; set; }
        public long? PaymentHistoryID { get; set; }
        public string? InvoiceID { get; set; }
        public long? ContactID { get; set; }
        public decimal? TotalAvailableCredits { get; set; }
        public int? NoOfJobPosted { get; set; }
        public int? NoOfVettedProfileView { get; set; }
        public int? NoOfNonVettedProfileView { get; set; }
        public string? CreditAmountValue { get; set; }
        public string? PlanHoverText { get; set; }
    }
}
