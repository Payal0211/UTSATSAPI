using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal_Result
    {
        public string? Title { get; set; }
        public string? HRNumber { get; set; }
        public string? Oppurtunity { get; set; }
        public string? YearsofExperience { get; set; }
        public string? EmploymentType { get; set; }
        public string? Budget { get; set; }
        public string? TimeZone { get; set; }
        public string? Skills { get; set; }
        public string? CreditUsed { get; set; }
        public string? HRCreatedDate { get; set; }
        public string? HRExpiryDate { get; set; }
        public string? ExpiryDays { get; set; }
        public string? ExpiryColor { get; set; }
        public string? VettedProfileCredits { get; set; }
        public string? NonVettedProfileCredits { get; set; }
        public string? CreditBalanceInfo { get; set; }
        public string? PostedBy { get; set; }
        public string? NewJobPostingDate { get; set; }
        public string? NewExpirationDate { get; set; }
        public string? AvailableCredits { get; set; }
        public string? JobPostingCredits { get; set; }
        public bool? ScreeningQuestionsGenerated { get; set; }
        public bool? IsBudgetAnnually { get; set; }
        public bool? IsAssessmentSharedOneTime { get; set; }
        public long? AssessmentID { get; set; }
        public string? ScreeningQuestionLink { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public int? HRTypeID { get; set; }
        public string? RepostingMessage { get; set; }

    }
}
