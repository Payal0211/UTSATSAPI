using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactInterviewFeedback
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public string? FeedBackType { get; set; }
        public decimal? RateTalentCurrentRole { get; set; }
        public string? AnsTalentCurrentRole { get; set; }
        public decimal? RateInterpretationCommunicationSkills { get; set; }
        public string? AnsInterpretationCommunicationSkills { get; set; }
        public decimal? RateProfessionalism { get; set; }
        public string? AnsProfessionalism { get; set; }
        public decimal? RateTalentfittoCulture { get; set; }
        public string? AnsTalentfittoCulture { get; set; }
        public decimal? RateCompanyCulture { get; set; }
        public string? AnsCompanyCulture { get; set; }
        public string? NohireTalentDontFitCurrentRole { get; set; }
        public string? NohireTalentImprove { get; set; }
        public bool? NohireReconsiderHiringTalent { get; set; }
        public string? OnHoldDescription { get; set; }
        public string? RescheduledDescription { get; set; }
        public string? TechnicalSkillRating { get; set; }
        public string? CommunicationSkillRating { get; set; }
        public string? CognitiveSkillRating { get; set; }
        public string? TopSkill { get; set; }
        public string? ImprovedSkill { get; set; }
        public string? MessageToTalent { get; set; }
        public bool? IsNextroundDetailsSubmitted { get; set; }
        public string? ClientsDecision { get; set; }
        public string? Comments { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
