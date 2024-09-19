using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAibasedHrtalentMatchMakingDetail
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public int? MatchingSubFactorId { get; set; }
        public long? MatchingFactorCalcId { get; set; }
        public string? MatchingFactorDisplayValue { get; set; }
        public decimal? MatchingFactorCalcValue { get; set; }
        public decimal? FactorWiseCalcWeightage { get; set; }
        public decimal? CalcWeightage { get; set; }
        public int? TotalHrskills { get; set; }
        public long? HiringRequestIds { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public long? AiId { get; set; }
    }
}
