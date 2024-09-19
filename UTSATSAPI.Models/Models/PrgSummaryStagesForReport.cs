using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSummaryStagesForReport
    {
        public int Id { get; set; }
        public string SummaryStage { get; set; } = null!;
        public string? CurrentStageActionId { get; set; }
        public string? NextStageActionId { get; set; }
        public string? StageVersion { get; set; }
        public int? OrderSequence { get; set; }
        public int? Sladays { get; set; }
        public string? CurrentStageExtraActions { get; set; }
        public string? NextStageExtraActions { get; set; }
    }
}
