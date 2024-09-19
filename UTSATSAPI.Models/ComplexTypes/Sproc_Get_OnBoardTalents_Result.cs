using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_OnBoardTalents_Result
    {
        public long? ID { get; set; }
        public string? EngagemenID { get; set; }
        public string? Position { get; set; }
        public string? Client { get; set; }
        public string? Talent { get; set; }
        public string? OnboardingTalent { get; set; }
        public string? OnboardingClient { get; set; }
        public string? LegalTalent { get; set; }
        public string? LegalClient { get; set; }
        public string? KickOff { get; set; }
        public string? CreatedByDatetime { get; set; }
        public int? TotalRecords { get; set; }
        public int? TalentOnBoarding_StatusID { get; set; }
        public int? ClientOnBoarding_StatusID { get; set; }
        public int? TalentLegal_StatusID { get; set; }
        public int? ClientLegal_StatusID { get; set; }
        public int? Kickoff_StatusID { get; set; }
        public bool? IsTalentRaiseConcern { get; set; }
        public bool? IsHRManaged { get; set; }
        public int? IsAMAssigned { get; set; }
        public string? HR_Number { get; set; }
        public long? HiringId { get; set; }
        public string? Company { get; set; }
        public string? AMAssignmentuser { get; set; }
        public string? OldTalent { get; set; }
        public int? NoticePeriod { get; set; }
        public int? IsLost { get; set; }
        public string? ReplacementDate { get; set; }
        public string? LastWorkingDate { get; set; }
        public int? HR_Status { get; set; }
        public long? IsInvoiceCreated { get; set; }
        public decimal? Final_HR_Cost { get; set; }
        public decimal? Talent_Cost { get; set; }
        public decimal? NRPercentage { get; set; }
        public string? SalesPerson { get; set; }
        public int? IsContractCompleted { get; set; }
        public string? ContractStartDate { get; set; }
        public string? ContractEndDate { get; set; }
        public int? ContractDuration { get; set; }
        public string? ContractStatus { get; set; }
        public string? ReplacementEng { get; set; }
        public int? GEO_ID { get; set; }
        public long? FeedbackId { get; set; }
        public decimal? DPAmount { get; set; }
        public decimal? DP_Percentage { get; set; }
        public string? TypeOfHR { get; set; }
        public bool? IsPartnerHR { get; set; }
        public int? HR_StatusID { get; set; }
        public string? TalentOnBoardDate { get; set; }
        public string? TSCName { get; set; }
    }
}
