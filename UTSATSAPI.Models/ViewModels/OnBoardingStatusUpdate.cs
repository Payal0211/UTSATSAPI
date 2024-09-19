using System.Diagnostics.CodeAnalysis;

namespace UTSATSAPI.Models.ViewModels
{
    public class OnBoardingLegalUpdates
    {
        public long? OnBoardID { get; set; }
        public long? TalentID { get; set; }
        public long? HiringRequestID { get; set; }
        public long? ContactID { get; set; }
        public long? CompanyID { get; set; }
        public string? InvoiceRaiseTo { get; set; }
        public string? InvoiceRaiseToEmail { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ClientSOWSignDate { get; set; }
        public DateTime? TalentSOWSignDate { get; set; }
        public DateTime? ClientMSASignDate { get; set; }
        public DateTime? TalentMSASignDate { get; set; }
        public bool? IsIndefiniteHR { get; set; }
        public DateTime? JoiningDate { get; set; }        
        public bool? IsReplacement { get; set; }
        public TalentReplacement talentReplacement { get; set; } = new TalentReplacement();
    }
    public class OnBoardingStatusUpdate
    {
        public long OnboardID { get; set; }
        public long TalentID { get; set; }
        public long HiringRequestID { get; set; }
        public long ContactID { get; set; }
        public string Action { get; set; }
        public OnboardingTalent? OnboardingTalent { get; set; } 
        public OnboardingClient? OnboardingClient { get; set; }
        public LegalTalent? LegalTalent { get; set; }
        public LegalClient? LegalClient { get; set; }
        public KickOff? KickOff { get; set; }
        public AfterKickOff? AfterKickOff { get; set; }
        //public string BearerToken { get; set; }

        // UTS-7389: Get the replacement details from UI.
        public bool? IsReplacement { get; set; }
        public TalentReplacement talentReplacement { get; set; } = new TalentReplacement();
    }
    public class OnboardingTalent
    {
        public int TalentOnBoardingStatusID { get; set; }
        public string? TalentConcernRemark { get; set; }
    }
    public class OnboardingClient
    {
        public int ClientOnBoardingStatusID { get; set; }
        public string? ClientConcernRemark { get; set; }
    }
    public class LegalTalent
    {
        public int TalentLegalStatusID { get; set; }
        public DateTime? TalentLegalDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
    }
    public class LegalClient
    {
        public int ClientLegalStatusID { get; set; }
        public DateTime? ClientLegalDate { get; set; }
        public int? TotalDuration { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? Lastworkingdate { get; set; }
        public int? CompanyLegalDocID { get; set; }
        public DateTime? MSASignDate { get; set; }
        public string? SOWDocumentLink { get; set; }
    }
    public class KickOff
    {
        public int KickoffStatusID { get; set; }
        public int? KickoffTimezonePreferenceId { get; set; }
        public DateTime? KickoffDate { get; set; }
        public bool IsAfterKickOff { get; set; }
    }

    public class AfterKickOff
    {
        public string? ZohoInvoiceNumber { get; set; }
        public string? InvoiceCurrency { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? TalentOnBoardDate { get; set; }
    }
}
