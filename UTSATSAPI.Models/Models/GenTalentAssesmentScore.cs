using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentAssesmentScore
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public decimal? VersantScore { get; set; }
        public decimal? MettleScore { get; set; }
        public decimal? CognitiveAbilities { get; set; }
        public int? CognitiveBenchmarkId { get; set; }
        public decimal? NumericalAbilities { get; set; }
        public int? NumericalBenchmarkId { get; set; }
        public decimal? DataAnalysis { get; set; }
        public int? DataAnalysisBenchmarkId { get; set; }
        public decimal? NumericalAbility { get; set; }
        public int? NumericalAbilityBenchmarkId { get; set; }
        public decimal? LogicalReasoning { get; set; }
        public int? LogicalReasoningBenchmarkId { get; set; }
        public decimal? AbstractReasoning { get; set; }
        public int? AbstractReasoningBenchmarkId { get; set; }
        public decimal? AttentionToDetails { get; set; }
        public int? AttentionToDetailsBenchmarkId { get; set; }
        public decimal? CodilityScore { get; set; }
        public decimal? HackerRank { get; set; }
        public decimal? InterviewScore { get; set; }
        public decimal? InterviewPercentage { get; set; }
        public int? InterviewTakenBy { get; set; }
        public string? VersantSummary { get; set; }
        public string? VersantScoreUrl { get; set; }
        public string? VersantScoreFile { get; set; }
        public string? MettleScoreUrl { get; set; }
        public string? MettleScoreFile { get; set; }
        public decimal? TotalScore { get; set; }
        public decimal? Rank { get; set; }
        public string? Status { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsActive { get; set; }
        public string? CodilityUrl { get; set; }
        public string? CodilityFile { get; set; }
        public string? VersantOverAll { get; set; }
        public string? VersantCandidateCapabilities { get; set; }
        public string? HackerRankUrl { get; set; }
        public string? HackerRankFile { get; set; }
        public decimal? SecondarySkillInterviewPercentage { get; set; }
        public string? InterviewUrl { get; set; }
        public string? CodilityDescription { get; set; }
        public string? HackerRankDescription { get; set; }
        public string? InterviewDescription { get; set; }
        public bool? IsHiremeeChecked { get; set; }
        public decimal? HiremeeScore { get; set; }
        public string? HiremeeDescription { get; set; }
        public string? HiremeeUrl { get; set; }
        public string? HiremeeFile { get; set; }
        public bool? IsCodility { get; set; }
        public bool? IsHackerRank { get; set; }
    }
}
