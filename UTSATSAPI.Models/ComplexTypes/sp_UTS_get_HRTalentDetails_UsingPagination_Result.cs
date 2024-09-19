using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_get_HRTalentDetails_UsingPagination_Result
    {
        public string? Name { get; set; }
        public string? BillRate { get; set; }
        public string? PayRate { get; set; }
        public decimal CTP_TalentCost { get; set; }
        public string? Status { get; set; }
        public string? NR { get; set; }
        public string? TalentTimeZone { get; set; }
        public string? NoticePeriod { get; set; }
        public int TalentStatusID_BasedOnHR { get; set; }
        public long TalentID { get; set; }
        public long HiringDetailID { get; set; }
        public int InterViewStatusId { get; set; }
        public long IsdisplaySelectTimeslot { get; set; }
        public long SelectedInterviewId { get; set; }
        public string? InterviewStatus { get; set; }
        public string? InterviewROUND { get; set; }
        public string? InterviewDateTime { get; set; }
        public string? Slotconfirmed { get; set; }
        public long ContactId { get; set; }
        public long MasterId { get; set; }
        public long Shortlisted_InterviewID { get; set; }
        public int ClientOnBoarding_StatusID { get; set; }
        public long OnBoardId { get; set; }
        public string? ClientOnBoardStatus { get; set; }
        public int Kickoff_StatusID { get; set; }
        public int LegalTalentOnBoarding_StatusID { get; set; }
        public int LegalClientOnBoarding_StatusID { get; set; }
        public int TalentOnBoarding_StatusID { get; set; }
        public string? TalentOnBoardStatus { get; set; }
        public string? ClientFeedback { get; set; }
        public string? SlotGivenStatus { get; set; }
        public bool IsLaterSlotGiven { get; set; }
        public long NextRound_InterviewDetailsID { get; set; }
        public string? TalentOnBoardDate { get; set; }
        public string? HR_Number { get; set; }
        public string? RequestStatus { get; set; }
        public string? TalentSource { get; set; }
        public string? TalentRole { get; set; }
        public string? TotalExpYears { get; set; }
        public int ProfileStatusCode { get; set; }
        public long ContactPriorityID { get; set; }
        public string? ATSTalentLiveURL { get; set; }
        public string? ATSNonNDAURL { get; set; }
        public string? DPPercentage { get; set; }
        public decimal DPAmount { get; set; }
        public decimal BillRateDP { get; set; }
        public string? PayRateDP { get; set; }
        public string? EngagemenID { get; set; }
        public int IsAMAssigned { get; set; }
        public long? ClientFeedbackID { get; set; }
        public string? PreferredAvailability { get; set; }
        public string? RejectedReason { get; set; }
        public string? OnHoldReason { get; set; }
        public string? CancelledReason { get; set; }
        public string? TalentOtherReason { get; set; }
        public string? TalentRemarks { get; set; }
        public string? TalentInterviewStatus { get; set; }
        public string? InterviewStatusCode { get; set; }
        public string? TalentCurrenyCode { get; set; } //Represents the currency code of the talent.
        public string? CurrencySign { get; set; } //Represents the currency sign of the talent.
        public string? TalentPOCName { get; set; } //Represents the talentPocName.
        public bool? IsHRTypeDP { get; set; } //Represents whether the HR is dp or not.
        public int? TSC_PersonID { get; set; }
        public string? EmailID { get; set; }
        public string? UplersfeesAmount { get; set; }
        public string? ScheduleTimeZone { get; set; }
        public int? IsShownTalentStatus { get; set; }
        public string? TalentResumeLink { get; set; }
        public bool? NeedToCallAWSBucket { get; set; }
        public long? ATSTalentID { get; set; }
        public int? TotalRecords { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? ContractStartdate { get; set; }
        public string? ContractEnddate { get; set; }
        public string? JoiningDate { get; set; }
        public string? LastWorkingDate { get; set; }
    }
}
